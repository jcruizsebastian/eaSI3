import '../css/agenda.css';
import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";
import LoginSi3 from './LoginSi3';



interface JiraIssues {
    issueSi3: string;
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
}
interface AgendaState {
    weekissues: WeekJiraIssues[];
    link: String;

}
export class Agenda extends React.Component<    WeekJiraIssuesProps, AgendaState> {

    constructor(props: WeekJiraIssuesProps) {
        super(props);

  
        this.timeReassignment = this.timeReassignment.bind(this);
        this.state = { weekissues: this.props.weekissues, link: "https://jira.openfinance.es/browse/"};
    }

    public timeReassignment(event: React.FormEvent<HTMLInputElement>) {

        let day = event.currentTarget.id.split('|')[1];
        let issueId = event.currentTarget.id.split('|')[0];
        
        for (var dayIssue of this.state.weekissues) {
            if (new Date(dayIssue.fecha.toString()).getDate().toString() == day) {
                for (var issue of dayIssue.issues) {
                    if (issue.issueKey == issueId)
                        issue.tiempo = Number (event.currentTarget.value);
                }
            }
        }
        
       
        this.props.onAgendaModified(this.state.weekissues);
    }

   

    public render() {
        
        return <div>
            <table className="table table-condensed table-bordered">
                <thead>
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
                                            <input type="text" id={issue.issueKey + '|' + new Date(Weekissue.fecha.toString()).getDate()} name="tbTiempoCorregido" value={issue.tiempoCorregido} placeholder={String(issue.tiempo)} onChange={this.timeReassignment} />
                                        </div>
                                    )}
                                </td>
                              
                            </tr>
                            
                        ) 
                    }
                </tbody>
            </table>
            <br />
            
        </div>


    }

}

export default Agenda