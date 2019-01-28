import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";


interface JiraIssues {
    issueId: string;
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
export class Agenda extends React.Component<WeekJiraIssuesProps, WeekJiraIssuesProps> {

    constructor(props: WeekJiraIssuesProps) {
        super(props);

        this.prueba = this.prueba.bind(this);

        this.state = { weekissues: this.props.weekissues };
    }

    public prueba(event: React.FormEvent<HTMLInputElement>) {

        let day = event.currentTarget.id.split('-')[1];
        let issueId = event.currentTarget.id.split('-')[0];

        for (let dayIssue of this.state.weekissues) {
            if (new Date(dayIssue.fecha.toString()).getDate().toString() == day) {
                for (let issue of dayIssue.issues) {
                    if (issue.issueId == issueId)
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
                        <th>Tareas</th>
                        <th>Horas</th>
                    </tr>
                </thead>
                <tbody>
                    {

                        this.props.weekissues.map(Weekissue =>
                            <tr>
                                <td className="agenda-date active">
                                    <div className="shortdate text-muted">{new Date(Weekissue.fecha.toString()).toLocaleDateString()}</div>
                                </td>
                                <td className="agenda-events">
                                    {Weekissue.issues.map(issue => <div> {issue.titulo} </div>)}
                                </td>
                                <td className="agenda-events">
                                    {Weekissue.issues.map(issue => <div> {issue.tiempo} </div>)}
                                </td>
                                <td className="agenda-events">
                                    {Weekissue.issues.map(issue => <input type="text" id={issue.issueId + '-' + new Date(Weekissue.fecha.toString()).getDate()} name="tbTiempoCorregido" value={issue.tiempoCorregido} placeholder={issue.tiempo} onChange={this.prueba} />)}
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