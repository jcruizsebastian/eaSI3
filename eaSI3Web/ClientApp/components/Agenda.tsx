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
    onAgendaModified: Function;
    //calculateTotalHours: Function;
}
export interface AgendaState {
    weekissues: WeekJiraIssues[];
    link: String;
    changes: boolean;
}

export class Agenda extends React.Component<WeekJiraIssuesProps, AgendaState> {

    constructor(props: WeekJiraIssuesProps) {
        super(props)

        this.timeReassignment = this.timeReassignment.bind(this);
        this.calculateTotalHours = this.calculateTotalHours.bind(this);
        this.handleSubmitChanges = this.handleSubmitChanges.bind(this);

        this.state = { weekissues: this.props.weekissues, link: "https://jira.openfinance.es/browse/", changes: true };
    }

        public timeReassignment(event: React.ChangeEvent<HTMLInputElement>) {
        
        let day = event.currentTarget.id.split('|')[1];
        let issueId = event.currentTarget.id.split('|')[0];
        
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

        this.forceUpdate(); 
    }
    

    public handleSubmitChanges(e: { preventDefault: () => void; }) {
        alert("¿ Guardar cambios ?");       
        this.props.onAgendaModified(e);        
    }

    private calculateTotalHours() {

        let total = 0;
        let tiempo: number;

        let WeekJiraIssues = this.state.weekissues;
        for (let weekIssue of WeekJiraIssues) {
            for (let Issue of weekIssue.issues) {
                tiempo = Number(Issue.tiempo);
                total += tiempo;
            }
        }
        
        return total;
    }  

    public render() {

        let total: number = this.calculateTotalHours();

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

                        this.state.weekissues.map(Weekissue =>

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
                                        <div className="agenda-events-title" key={issue.titulo + issue.issueCode}>
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
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td><form className="dataForm" onSubmit={this.handleSubmitChanges}>
                            <input  type="submit" className="btn btn-secondary" value="Confirmar Cambios" />
                        </form>
                         </td>
                    </tr>
                </tbody>
            </table>
            <br />
            
        </div>


    }

}

export default Agenda;