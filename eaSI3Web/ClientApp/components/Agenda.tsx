import '../css/agenda.css';
import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";
import Login from './Login';


interface JiraIssues {
    issueSI3Code: string;
    issueCode: string;
    issueKey: string;
    titulo: string;
    tiempo: number;
    tiempoCorregido: number;
}

interface WeekJiraIssues {
    fecha: Date;
    issues: JiraIssues[];
}


interface WeekJiraIssuesProps {
    weekissues: WeekJiraIssues[];
    //onAgendaModified: Function;
    //calculateTotalHours: Function;
}
interface AgendaState {
    weekissues: WeekJiraIssues[];
    link: String;
    error: boolean;
}
export class Agenda extends React.Component<WeekJiraIssuesProps, AgendaState> {

    constructor(props: WeekJiraIssuesProps) {
        super(props);


        //this.timeReassignment = this.timeReassignment.bind(this);
        //this.onLoginSi3 = this.onLoginSi3.bind(this);
        this.isDisabledBtnSi3 = this.isDisabledBtnSi3.bind(this);
        this.state = { weekissues: this.props.weekissues, link: "https://jira.openfinance.es/browse/", error: false };
    }

     timeReassignment = (event: React.FormEvent<HTMLInputElement>) => {
        
        let day = event.currentTarget.id.split('|')[1];
        let issueId = event.currentTarget.id.split('|')[0];
        console.log("estoy dentro");

        for (var dayIssue of this.state.weekissues) {
            if (new Date(dayIssue.fecha.toString()).getDate().toString() == day) {
                for (var issue of dayIssue.issues) {
                    if (issue.issueKey == issueId) {
                        issue.tiempo = Number(event.currentTarget.value);
                        break;
                    }

                }
            }
        }

        //this.props.onAgendaModified(this.state.weekissues);
    }

    private isDisabledBtnSi3() {

        let total = 0;
        let tiempo: number;

        let WeekJiraIssues = this.props.weekissues;
        for (let weekIssue of WeekJiraIssues) {
            for (let Issue of weekIssue.issues) {
                tiempo = Number(Issue.tiempo);
                total += tiempo;
                if (tiempo % 1 != 0) {
                    return true;
                }
            }
        }
        //this.setState({ totalHours: total });

        if (total <= 40) { return false; }
        else { return true; }

    }

    /*
    private onLoginSi3(e: { preventDefault: () => void; }, user: string, password: string, checked: boolean) {

        user = user.replace("'", " ").trim();
        e.preventDefault();

        if (checked) { localStorage.setItem("userSi3", user); localStorage.setItem("passwordSi3", password); }

        fetch('api/SI3/register?username=' + user + '&password=' + password + '&selectedWeek=' + this.state.selectedWeek + '&totalHours' + this.state.totalHours, {
            method: 'post',
            body: JSON.stringify(this.state.Weekissues),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(response => response.json() as Promise<JiraResponse>)
            .then(data => {
                if (data.message) {
                    alert(data.message);
                }
                else
                    alert("Horas imputadas en SI3");
            });


    }
    */

    public render() {
        /*
        let si3 = <div> <h3>Ingrese credenciales de SI3</h3> <Login onLogin={this.onLoginSi3} isDisabled={this.isDisabledBtnSi3}
            userProps={localStorage.getItem("userSi3") as string} passwordProps={localStorage.getItem("passwordSi3") as string} /> </div>;
        */
        let total: number = this.props.calculateTotalHours();
        console.log("entra en el render de agenda")
        return <div>

            <table className="table .table-responsive">
                <caption>Lista de tareas Jira</caption>
                <thead className="thead-dark">
                    <tr>
                        <th>Fecha</th>
                        <th>ID</th>
                        <th>Tareas</th>
                        <th>Horas</th>
                    </tr>
                </thead>
                <tbody>
                    {

                        this.props.weekissues.map(Weekissue =>

                            <tr key={Weekissue.fecha.toString()} >
                                <td className="agenda-date active">
                                    <div className="shortdate text-muted" > {new Date(Weekissue.fecha.toString()).toLocaleDateString()}</div>
                                </td>

                                <td className="agenda-events" >
                                    {Weekissue.issues.map(issue =>
                                        <div className="agenda-events-id" key={issue.issueCode}>
                                            <a target="_blank" href={this.state.link.concat(issue.issueKey)}>{issue.issueKey}</a>
                                            <label className="issue-si3">({issue.issueSI3Code})</label>
                                        </div>
                                    )}

                                </td>

                                <td className="agenda-events">
                                    {Weekissue.issues.map(issue =>
                                        <div className="agenda-events-title" key={issue.titulo}>
                                            <label className="agenda-events-label" title={issue.titulo}> {issue.titulo}</label>
                                        </div>
                                    )}
                                </td>


                                <td className="agenda-events">
                                    {Weekissue.issues.map(issue =>
                                        <div className="agenda-events-hours" key={issue.issueKey}>
                                            <input type="text" id={issue.issueKey + '|' + new Date(Weekissue.fecha.toString()).getDate()} name="tbTiempoCorregido" value={issue.tiempoCorregido}
                                                placeholder={String(issue.tiempo)} onChange={this.timeReassignment} className={(Number(issue.tiempo) % 1 != 0) ? "invalid" : "valid"} />

                                        </div>
                                    )}
                                </td>

                            </tr>

                        )
                    }

                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td><label className="agenda-total">Total : {total.toString()}</label></td>
                    </tr>
                </tbody>
            </table>
            <br />
            
        </div>


    }

}

export default Agenda