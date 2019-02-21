import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";
import Agenda from './Agenda';
import { AgendaState } from './Agenda';
import Login from './Login';
import Loader from 'react-loader-advanced';
import { Layout } from './Layout';

//import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";

interface UserCredentials {
    Weekissues: WeekJiraIssues[];
    loadedJira: boolean;
    loadingJira: boolean;
    calendar: Calendar;
    calendarLoaded: boolean;
    selectedWeek?: string;
    todoOk: boolean;
    loading: boolean;
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
        this.isTodoOk = this.isTodoOk.bind(this);       
        this.getWeekofYear = this.getWeekofYear.bind(this);
        this.handleChangeWeek = this.handleChangeWeek.bind(this);
        this.getCookie = this.getCookie.bind(this);

        this.state = {
            Weekissues: [], loadedJira: false, loadingJira: false, calendar: { weeks: [] }, calendarLoaded: false, todoOk: false, loading: false
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

    private onLoginJira(e: { preventDefault: () => void; }) {
        
        //user = user.replace(" ", " ").trim();
        e.preventDefault();
        
        //if (checked) { localStorage.setItem("userJira", user); localStorage.setItem("passwordJira", password); }

        this.setState({ loadingJira: true , loading: true});

        fetch('api/Jira/worklog?username=' + this.getCookie("userJira") + '&password=' + this.getCookie("passJira") + '&selectedWeek=' + this.state.selectedWeek)
           
             .then(response => response.json() as Promise<WeekJiraIssuesResponse>)
             .then(data => {
                   if (data.notOk) {
                       alert(data.message);
                       this.setState({ loadingJira: false, loadedJira: false, loading: false });
                   }
                   else if (data.weekJiraIssues.length == 0) {
                        alert("No hay tareas en Jira");
                       this.setState({ loadingJira: false, loadedJira: false, loading: false});

                   }
                   else
                       this.setState({ Weekissues: data.weekJiraIssues, loading: false }, this.confirmLoadedJira);
                    });

        
    }

    //función para sacar las cookies, cname => userJira, passJira ... etc.
    public getCookie(cname:String) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

    private onLoginSi3(e: { preventDefault: () => void; }, user: string, password: string, checked: boolean) {
        
        user = user.replace("'", " ").trim();
        e.preventDefault();
        this.setState({ loading: true });
        let agenda = (this.refs["agenda1"] as React.Component<{}, AgendaState>);
        
        if (checked) { localStorage.setItem("userSi3", user); localStorage.setItem("passwordSi3", password); }

        let total = 0;
        let WeekJiraIssues = agenda.state.weekissues;
        for (let weekIssue of WeekJiraIssues) {
            for (let Issue of weekIssue.issues) {
                total += Number(Issue.tiempo);
            }
        }

        fetch('api/SI3/register?username=' + user + '&password=' + password + '&selectedWeek=' + this.state.selectedWeek + '&totalHours=' + total, {
            method: 'post',
            body: JSON.stringify(agenda.state.weekissues),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
        })
            .then(response => response.json() as Promise<String>)    
            .then(data => {
                if (data.length > 0) {
                        alert(data);
                        this.setState({ loading: false });
                    }
                    else {
                        alert("Horas imputadas en SI3");
                        this.setState({ loading: false });}
                });
        
        
    }



    public handleChangeWeek(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ selectedWeek: event.currentTarget.value });

    }

    public isTodoOk(val: boolean) { this.setState({ todoOk: val }); }

    public render() {
        console.log("Entra en render de Home");
       
        let agenda;
        let si3;
        let jira;
        let calendar;

        if (this.state.calendarLoaded) {
            jira = <input type="button" value="Obtener issues" className="btn btn-primary" onClick={this.onLoginJira} />

            calendar = <div>
                <label>Elija semana de trabajo :</label>
                <select className="custom-select" onChange={this.handleChangeWeek} /*value={this.state.calendar.weeks.length}*/>
                    {
                        this.state.calendar.weeks.map(week =>
                            <option value={week.numberWeek} key={week.numberWeek} >
                                {week.description}
                            </option>)
                    }
                </select>
            </div>
        }
        if (this.state.loadedJira) {

            agenda = <Agenda weekissues={this.state.Weekissues} ref="agenda1" isTodoOk={this.isTodoOk}/>

            si3 = <div> <input type="button" id= "btnSi3" value="Imputar tareas en Si3" className="btn btn-primary" disabled={this.state.todoOk} /></div>;
          
        }


        const spinner = <span><ReactLoading color='#fff' type='spin' className="spinner" height={128} width={128}  /></span>

        return (
            
            
            <div>
               
                {calendar}
                {jira}   
                {agenda}   
                {si3}
                <Loader show={this.state.loading} message={spinner} hideContentOnLoad={false} className={(this.state.loading==true) ? "overlay": "overlay-1"} />
            </div>
                
            
        )
                    
    }
}
export default Home


