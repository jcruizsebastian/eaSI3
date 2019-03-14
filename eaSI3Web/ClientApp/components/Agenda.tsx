import 'isomorphic-fetch';
import * as React from 'react';
import '../css/agenda.css';
import { WeekJiraIssuesProps } from './Model/Props/WeekJiraIssuesProps';
import { AgendaState } from './Model/States/AgendaState';
import { PopupVincularTarea } from './popupVincularTarea';

export class Agenda extends React.Component<WeekJiraIssuesProps, AgendaState> {

    constructor(props: WeekJiraIssuesProps) {
        super(props)

        this.isDisabledBtnSi3 = this.isDisabledBtnSi3.bind(this);
        this.timeReassignment = this.timeReassignment.bind(this);
        this.calculateTotalHours = this.calculateTotalHours.bind(this);
        this.vincular = this.vincular.bind(this);
        this.closePopup = this.closePopup.bind(this);
        this.state = { weekissues: this.props.weekissues, link: "https://jira.openfinance.es/browse/", vincular: false, issueVincular:"" };
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

        if (total <= this.props.availableHours) { return false; }
        else { return true; }
    }

    public vincular(issuekey: string) {
        this.setState({ vincular: true, issueVincular: issuekey });       
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
    public closePopup(idSi3: string, key: string) {
        if (idSi3.length > 0) {
            let WeekJiraIssues = this.props.weekissues;
            for (let weekIssue of WeekJiraIssues) {
                for (let Issue of weekIssue.issues) {
                    if (Issue.issueSI3Code == null && Issue.issueKey == key) {
                        Issue.issueSI3Code = idSi3;
                    }
                }
            }
        }
        this.setState({ vincular: false });
    }
    public render() {

        let total: number = this.calculateTotalHours();
    
        return <div>
            {this.state.vincular ?
                <PopupVincularTarea keyJira={this.state.issueVincular} closePopup={this.closePopup} />
                : null
            }
            <div className="table-responsive">
            <table className="table">
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

                                <td className="agenda-events-td-id" >
                                    
                                    {Weekissue.issues.filter(issue => issue.issueSI3Code == null).map(
                                        x =>
                                            <div className="agenda-events-id" key={x.issueCode}>
                                                <a target="_blank" href={this.state.link.concat(x.issueKey)}>{x.issueKey}</a>
                                                <button type="button" id="btn-vincular" className="btn btn-danger btn-sm" onClick={() => { this.vincular(x.issueKey) }} >Vincular</button>
                                               
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

                                <td className="agenda-events-title">
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
                                                <input type="text" id={issue.issueKey + '|' + new Date(Weekissue.fecha.toString()).getDate()} name="tbTiempoCorregido" value={issue.tiempo}
                                                    placeholder={String(issue.tiempo)} onChange={this.timeReassignment} className={((Number(issue.tiempo) % 1 != 0) || (issue.issueSI3Code == null && issue.tiempo > 0)) ? "invalid" : "valid"}
                                                    autoComplete="off" />

                                            </div>
                                    )}
                                    {Weekissue.issues.filter(issue => issue.issueSI3Code != null).map(
                                        issue =>
                                            <div className="agenda-events-hours" key={issue.issueKey}>
                                                <input type="text" id={issue.issueKey + '|' + new Date(Weekissue.fecha.toString()).getDate()} name="tbTiempoCorregido" value={issue.tiempo}
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
                        <td><label className="agenda-total">Horas : {total.toString()}/{this.props.availableHours.toString()} </label></td>
                    </tr>
                </tbody>
                </table>
            </div>
            <br />
            
        </div>
    }
}

export default Agenda;