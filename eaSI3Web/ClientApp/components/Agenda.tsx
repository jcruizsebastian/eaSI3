import 'isomorphic-fetch';
import * as React from 'react';
import '../css/agenda.css';
import { WeekJiraIssuesProps } from './Model/Props/WeekJiraIssuesProps';
import { AgendaState } from './Model/States/AgendaState';

export class Agenda extends React.Component<WeekJiraIssuesProps, AgendaState> {

    constructor(props: WeekJiraIssuesProps) {
        super(props)

        this.isDisabledBtnSi3 = this.isDisabledBtnSi3.bind(this);
        this.timeReassignment = this.timeReassignment.bind(this);
        this.calculateTotalHours = this.calculateTotalHours.bind(this);
        this.state = { weekissues: this.props.weekissues, link: "https://jira.openfinance.es/browse/" };
    }

    componentDidMount() {
        var todoOk = this.isDisabledBtnSi3();
        this.props.isTodoOk(todoOk);
    }

    public timeReassignment(event: React.ChangeEvent<HTMLInputElement>) {
        let day = event.currentTarget.id.split('|')[1];
        let issueId = event.currentTarget.id.split('|')[0];

        for (var dayIssue of this.props.weekissues) {
            if (new Date(dayIssue.fecha.toString()).getDate().toString() == day) {
                for (var issue of dayIssue.issues) {
                    if (issue.issueKey == issueId) {
                        issue.tiempo = Number(event.currentTarget.value);
                        break;
                    }
                }
            }
        }

        var todoOk = this.isDisabledBtnSi3();
        this.props.isTodoOk(todoOk);

        this.forceUpdate();
    }

    private isDisabledBtnSi3() {

        let total = 0;
        let tiempo: number;

        let WeekJiraIssues = this.props.weekissues;
        for (let weekIssue of WeekJiraIssues) {
            for (let Issue of weekIssue.issues) {
                tiempo = Number(Issue.tiempo);
                total += tiempo;
                if ((tiempo % 1 != 0) || (Issue.issueSI3Code == null && Issue.tiempo > 0)) {
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

        let WeekJiraIssues = this.props.weekissues;
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

                        this.props.weekissues.map(Weekissue =>

                            <tr key={Weekissue.fecha.toString()} >
                                <td className="agenda-date active">
                                    <div className="shortdate text-muted" > {new Date(Weekissue.fecha.toString()).toLocaleDateString()}</div>
                                </td>

                                <td className="agenda-events" >
                                    {Weekissue.issues.filter(issue => issue.issueSI3Code == null).map(
                                        x =>
                                            <div className="agenda-events-id" key={x.issueCode}>
                                                <a target="_blank" href={this.state.link.concat(x.issueKey)}>{x.issueKey}</a>

                                                <label className="issue-si3">({x.issueSI3Code})</label>
                                            </div>
                                    )}

                                    {Weekissue.issues.filter(issue => issue.issueSI3Code != null).map(
                                        issue =>
                                            <div className="agenda-events-id" key={issue.issueCode}>
                                                <a target="_blank" href={this.state.link.concat(issue.issueKey)}>{issue.issueKey}</a>
                                                <label className="issue-si3">({issue.issueSI3Code})</label>
                                            </div>
                                    )
                                    }

                                </td>

                                <td className="agenda-events">
                                    {Weekissue.issues.filter(issue => issue.issueSI3Code == null).map(
                                        issue =>

                                            <div className="agenda-events-title" key={issue.titulo + issue.issueCode}>
                                                <label className="agenda-events-label" title={issue.titulo}> {issue.titulo}</label>
                                            </div>
                                    )}

                                    {Weekissue.issues.filter(issue => issue.issueSI3Code != null).map(
                                        x =>
                                            <div className="agenda-events-title" key={x.titulo + x.issueCode}>
                                                <label className="agenda-events-label" title={x.titulo}> {x.titulo}</label>
                                            </div>
                                    )}
                                </td>


                                <td className="agenda-events">

                                    {Weekissue.issues.filter(issue => issue.issueSI3Code == null).map(
                                        issue =>
                                            <div className="agenda-events-hours" key={issue.issueKey}>
                                                <input type="text" id={issue.issueKey + '|' + new Date(Weekissue.fecha.toString()).getDate()} name="tbTiempoCorregido" value={issue.tiempoCorregido}
                                                    placeholder={String(issue.tiempo)} onChange={this.timeReassignment} className={((Number(issue.tiempo) % 1 != 0) || (issue.issueSI3Code == null && issue.tiempo > 0)) ? "invalid" : "valid"}
                                                    autoComplete="off" />

                                            </div>
                                    )}
                                    {Weekissue.issues.filter(issue => issue.issueSI3Code != null).map(
                                        issue =>
                                            <div className="agenda-events-hours" key={issue.issueKey}>
                                                <input type="text" id={issue.issueKey + '|' + new Date(Weekissue.fecha.toString()).getDate()} name="tbTiempoCorregido" value={issue.tiempoCorregido}
                                                    placeholder={String(issue.tiempo)} onChange={this.timeReassignment} className={((Number(issue.tiempo) % 1 != 0) || (issue.issueSI3Code == null && issue.tiempo > 0)) ? "invalid" : "valid"}
                                                    autoComplete="off" />

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

export default Agenda;