import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";
import Agenda from './Agenda';
import Login from './Login';
//import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";

interface UserCredentials {
    Weekissues: WeekJiraIssues[];
    loadedJira: boolean;
    loadingJira: boolean;
}

interface WeekJiraIssues {
    fecha: Date;
    issues: JiraIssues[];
}

interface JiraIssues {
    issueSi3: string;
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

interface weekOfYear {
    number: Number;
}
export class Home extends React.Component<RouteComponentProps<{}>, UserCredentials> {

    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.agendaModified = this.agendaModified.bind(this);
        this.onLoginJira = this.onLoginJira.bind(this);
        this.onLoginSi3 = this.onLoginSi3.bind(this);
        this.confirmLoadedJira = this.confirmLoadedJira.bind(this);
        this.isDisabledBtnJira = this.isDisabledBtnJira.bind(this);
        this.isDisabledBtnSi3 = this.isDisabledBtnSi3.bind(this);
        this.calculateTotalHours = this.calculateTotalHours.bind(this);
        this.getWeekofYear = this.getWeekofYear.bind(this);

        
        this.state = { Weekissues: [], loadedJira: false, loadingJira: false };

        
    }

    private agendaModified(weekJiraIssues: WeekJiraIssues[])
    {
        this.setState({ Weekissues: weekJiraIssues });
    }

    private renderAgenda(Weekissues: WeekJiraIssues[]) {

        return <Agenda weekissues={Weekissues} onAgendaModified={this.agendaModified} calculateTotalHours={this.calculateTotalHours} />     

    }

    private confirmLoadedJira() {
        this.setState({

            loadingJira: false,
            loadedJira: true
            
        });
    }

    private getWeekofYear() {
        
        fetch('api/Jira/Weeks')
            .then(response => response.json() as Promise<string>)
            .then(data => {
                console.log(data);
            });
    }
    
    private onLoginJira(e: { preventDefault: () => void; }, user: string, password: string) {
            user = user.replace(" ", " ").trim();
            e.preventDefault();
            this.setState({ loadingJira: true });

        fetch('api/Jira/worklog?username=' + user + '&password=' + password)

            .then(response => response.json() as Promise<WeekJiraIssuesResponse>)
            .then(data => {
                if (data.notOk) {
                    alert(data.message);
                    this.setState({ loadingJira: false, loadedJira: false });
                }
                else if (data.weekJiraIssues.length == 0) {
                    alert("No hay tareas en Jira");
                    this.setState({ loadingJira: false, loadedJira: false });
                    this.getWeekofYear();
                }
                else
                    this.setState({ Weekissues: data.weekJiraIssues }, this.confirmLoadedJira);
            });
       
    }

    
    private onLoginSi3(e: { preventDefault: () => void; }, user: string, password: string) {

        user = user.replace("'", " ").trim();

        e.preventDefault();
        
            fetch('api/SI3/register?username=' + user + '&password=' + password, {
                method: 'post',
                body: JSON.stringify(this.state.Weekissues),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
            })
                .then(response => response.json() as Promise<JiraResponse>)    
                .then(data => {
                    if (data.message.length != 0)
                    {
                        alert(data.message);
                    }
                    else
                        alert("Horas imputadas en Jira");
                });
        
        
    }

    private isDisabledBtnJira() {
        if (this.state.loadedJira) { return true; }
        else return false;
    }

    private isDisabledBtnSi3() {

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
        
        if (total <= 40) { return false; }
            else { return true; }
        
    }

    private calculateTotalHours() {

        let total = 0;
        let tiempo: number;

        let WeekJiraIssues = this.state.Weekissues;
        for (let weekIssue of WeekJiraIssues) {
            for (let Issue of weekIssue.issues) {
                tiempo = Number(Issue.tiempo);
                total += tiempo;
            }
        }
        
        return total;
    }

    public render() {

       
        let agenda = <p><em>Sin datos</em></p>;
        let si3;

        if (this.state.loadedJira) {

            agenda = this.renderAgenda(this.state.Weekissues);
            si3 = <div> <h3>Ingrese credenciales de SI3</h3> <Login onLogin={this.onLoginSi3} isDisabled={this.isDisabledBtnSi3} /> </div>;
        }

        if (this.state.loadingJira)
            agenda = <ReactLoading color='#000' type='cylon' />

        

        return (

            <div>
                <div>
                    <h3>Ingrese credenciales de Jira</h3>
                    <Login onLogin={this.onLoginJira} isDisabled={this.isDisabledBtnJira} />
                {agenda}
                {si3}
                </div>
            </div>
        )
                    
    }
}
export default Home


