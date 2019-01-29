import '../css/agenda.css';
import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";


interface JiraIssues {
    issueSi3: string;
    issueCode: string;
    issueKey: string;
    titulo: string;
    tiempo: string;
    tiempoCorregido: string;
}


interface WeekJiraIssues {
    fecha: Date;
    issues: JiraIssues[];
}


interface WeekJiraIssuesProps {
    weekissues: WeekJiraIssues[];
    
}
interface AgendaState {
    weekissues: WeekJiraIssues[];
    link: String;

}
export class Agenda extends React.Component<WeekJiraIssuesProps, AgendaState> {

    constructor(props: WeekJiraIssuesProps) {
        super(props);

        
        this.prueba = this.prueba.bind(this);
        this.state = { weekissues: this.props.weekissues, link: "https://jira.openfinance.es/browse/"};
    }

    public prueba(event: React.FormEvent<HTMLInputElement>) {

        let day = event.currentTarget.id.split('-')[1];
        let issueId = event.currentTarget.id.split('-')[0];

        for (let dayIssue of this.state.weekissues) {
            if (new Date(dayIssue.fecha.toString()).getDate().toString() == day) {
                for (let issue of dayIssue.issues) {
                    if (issue.issueKey == issueId)
                        issue.tiempo = event.currentTarget.value;
                }
            }
        }
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
                            <tr>
                                <td className="agenda-date active">
                                    <div className="shortdate text-muted" key={this.props.weekissues.indexOf(Weekissue)}>{new Date(Weekissue.fecha.toString()).toLocaleDateString()}</div>
                                </td>
                                
                                <td className="agenda-events">

                                    {Weekissue.issues.map(issue => <div className="agenda-events-id"> <a target="_blank" href={this.state.link.concat(issue.issueKey)}>{issue.issueKey}</a></div>)}
                                   
                                 </td>
                                
                                <td className="agenda-events">
                                    {Weekissue.issues.map(issue => <div className="agenda-events-title"> <label className = "agenda-events-label" title={issue.titulo}> {issue.titulo}</label> </div>)}
                                </td>
                                
                                <td className="agenda-events">
                                    {Weekissue.issues.map(issue => <div className="agenda-events-hours"> <input type="text" id={issue.issueKey + '-' + new Date(Weekissue.fecha.toString()).getDate()} name="tbTiempoCorregido" value={issue.tiempoCorregido} placeholder={issue.tiempo} onChange={this.prueba} /></div>)}
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