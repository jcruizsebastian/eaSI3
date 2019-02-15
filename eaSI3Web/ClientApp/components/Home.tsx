import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";
import Agenda from './Agenda';
import { AgendaState } from './Agenda';
import Login from './Login';
//import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";

interface UserCredentials {
    Weekissues: WeekJiraIssues[];
    WeekissuesNuevas: WeekJiraIssues[];
    loadedJira: boolean;
    loadingJira: boolean;
    calendar: Calendar;
    calendarLoaded: boolean;
    selectedWeek?: string;
    totalHours: number;
}

interface WeekJiraIssues {
    fecha: Date;
    issues: JiraIssues[];
}

interface JiraIssues {
    issueSI3Code: string;
    issueCode: string;
    issueKey: string;
    titulo: string;
    tiempo: number;
    tiempoCorregido: number;
}

interface WeekJiraIssuesResponse {
    weekJiraIssues: WeekJiraIssues[];
    notOk: boolean;
    message: string;
}

interface JiraResponse {
    message: string;
}

interface CalendarWeeks {
    numberWeek: number;
    description: string;
    starOfWeek: Date;
    endOfWeek: Date;
}

interface Calendar {
    weeks: CalendarWeeks[];
}
export class Home extends React.Component<RouteComponentProps<{}>,UserCredentials> {

    

    constructor(props: RouteComponentProps<{}>) {
        super(props);
        
        

        this.onLoginJira = this.onLoginJira.bind(this);
        this.onLoginSi3 = this.onLoginSi3.bind(this);
        this.confirmLoadedJira = this.confirmLoadedJira.bind(this);
        this.isDisabledBtnJira = this.isDisabledBtnJira.bind(this);
        this.isDisabledBtnSi3 = this.isDisabledBtnSi3.bind(this);
        
        this.getWeekofYear = this.getWeekofYear.bind(this);
        this.handleChangeWeek = this.handleChangeWeek.bind(this);

        this.state = {
            Weekissues: [], WeekissuesNuevas:[], loadedJira: false, loadingJira: false, calendar: { weeks: [] }, calendarLoaded: false, totalHours: 0
        };

        
    }
   
    componentDidMount() { this.getWeekofYear(); }

  

    private confirmLoadedJira() {
        this.setState({

            loadingJira: false,
            loadedJira: true
            
        });
    }

    private getWeekofYear() {
        
        fetch('api/Jira/Weeks')
            .then(response => response.json() as Promise<Calendar>)
            .then(data => {
                this.setState({ calendar: data, calendarLoaded: true, selectedWeek: data.weeks.length.toString() });  
            });
        
    }

    private onLoginJira(e: { preventDefault: () => void; }, user: string, password: string, checked: boolean) {

        user = user.replace(" ", " ").trim();
        e.preventDefault();
        
        if (checked) { localStorage.setItem("userJira", user); localStorage.setItem("passwordJira", password); }

        this.setState({ loadingJira: true });

        fetch('api/Jira/worklog?username=' + user + '&password=' + password + '&selectedWeek=' + this.state.selectedWeek)
           
             .then(response => response.json() as Promise<WeekJiraIssuesResponse>)
             .then(data => {
                   if (data.notOk) {
                        alert(data.message);
                        this.setState({ loadingJira: false, loadedJira: false });
                   }
                   else if (data.weekJiraIssues.length == 0) {
                        alert("No hay tareas en Jira");
                        this.setState({ loadingJira: false, loadedJira: false });

                   }
                   else
                        this.setState({ Weekissues: data.weekJiraIssues }, this.confirmLoadedJira);
                    });
   
    }


    private onLoginSi3(e: { preventDefault: () => void; }, user: string, password: string, checked: boolean) {
        
        user = user.replace("'", " ").trim();
        e.preventDefault();

        let agenda = (this.refs["agenda1"] as React.Component<{}, AgendaState>);
        
        if (checked) { localStorage.setItem("userSi3", user); localStorage.setItem("passwordSi3", password); }

        let total = 0;
        let tiempo: number;

        let WeekJiraIssues = agenda.state.weekissues;
        for (let weekIssue of WeekJiraIssues) {
            for (let Issue of weekIssue.issues) {
                tiempo = Number(Issue.tiempo);
                total += tiempo;
            }
        }

        fetch('api/SI3/register?username=' + user + '&password=' + password + '&selectedWeek=' + this.state.selectedWeek + '&totalHours' + total, {
            method: 'post',
            body: JSON.stringify(agenda.state.weekissues),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
            })
                .then(response => response.json() as Promise<JiraResponse>)    
                .then(data => {
                    if (data.message)
                    {
                        alert(data.message);
                    }
                    else
                        alert("Horas imputadas en SI3");
                });
        
        
    }

    private isDisabledBtnJira() {
       return false;
    }

    private isDisabledBtnSi3() {

        /*
        let total = 0;
        let tiempo: number;

        let WeekJiraIssues = this.state.Weekissues;
        for (let weekIssue of WeekJiraIssues) {
            for (let Issue of weekIssue.issues) {
                tiempo = Number(Issue.tiempo);
                total += tiempo;
                if (tiempo % 1 != 0) {
                    return true;
                }
            }
        }
        this.setState({ totalHours: total });
        if (total <= 40) { return false; }
            else { return true; }*/

        return false;
    }

    public handleChangeWeek(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ selectedWeek: event.currentTarget.value });

    }

    public render() {
        console.log("Entra en render de Home");
       
        let agenda = <p><em>Sin datos</em></p>;
        let si3;
        let jira;
        let calendar;

        if (this.state.calendarLoaded) {
            jira = <Login onLogin={this.onLoginJira} isDisabled={this.isDisabledBtnJira} 
                userProps={localStorage.getItem("userJira") as string} passwordProps={localStorage.getItem("passwordJira") as string} />
            calendar =<div>
                <label>Elija semana de trabajo :</label>
                <select className="custom-select" onChange={this.handleChangeWeek}>
                    {
                        this.state.calendar.weeks.map(week =>
                            <option value={week.numberWeek} key={week.numberWeek} selected={(week.numberWeek == this.state.calendar.weeks.length) ? true : false} >
                                {week.description}
                            </option>)
                    }
                </select>
            </div>
        }
        if (this.state.loadedJira) {

            agenda = <Agenda weekissues={this.state.Weekissues} ref="agenda1"/>

            si3 = <div> <h3>Ingrese credenciales de SI3</h3> <Login onLogin={this.onLoginSi3} isDisabled={this.isDisabledBtnSi3} 
                userProps={localStorage.getItem("userSi3") as string} passwordProps={localStorage.getItem("passwordSi3") as string}/> </div>;
        }

        if (this.state.loadingJira)
            agenda = <ReactLoading color='#000' type='cylon' />

        

        return (

            <div>
                <div>
                    {calendar}
                    <h3>Ingrese credenciales de Jira</h3>
                    {jira}
                    {agenda}
                    {si3}
                </div>
            </div>
        )
                    
    }
}
export default Home


