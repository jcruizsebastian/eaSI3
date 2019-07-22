import 'isomorphic-fetch';
import * as React from 'react';
import Loader from 'react-loader-advanced';
import ReactLoading from "react-loading";
import { Agenda } from './Agenda';
import { Calendar } from './Model/Calendar';
import { WeekJiraIssuesProps } from './Model/Props/WeekJiraIssuesProps';
import { AgendaState } from './Model/States/AgendaState';
import { UserCredentialsState } from './Model/States/UserCredentialsState';
import { WeekJiraIssues } from './Model/WeekJiraIssues';
import * as ReactDOM from 'react-dom';
import { Link } from 'react-router-dom';
import { Cube } from './Cube';
import { Popup } from './Popup';

interface WeekJiraIssuesResponse {
    weekJiraIssues: WeekJiraIssues[];
}

export class Home extends React.Component<{}, UserCredentialsState> {

    constructor(props: {}) {
        super(props);

        this.closePopup = this.closePopup.bind(this);
        this.onLoginJira = this.onLoginJira.bind(this);
        this.onLoginSi3 = this.onLoginSi3.bind(this);
        this.confirmLoadedJira = this.confirmLoadedJira.bind(this);
        this.isTodoOk = this.isTodoOk.bind(this);
        this.getWeekofYear = this.getWeekofYear.bind(this);
        this.isDisabledBtnSi3 = this.isDisabledBtnSi3.bind(this);
        this.getCookie = this.getCookie.bind(this);

        this.state = {
            Weekissues: [], loadedJira: false, loadingJira: false, calendar: { version: "", weeks: [] }, calendarLoaded: false, todoOk: false,
            loading: false, availableHours: 0, popup: false, popup_error: false, popup_data: []
        };
    }

    componentDidMount() {
        this.getWeekofYear();
    }

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
                this.onLoginJira();
            });
    }

    private onLoginJira() {
        
        //e.preventDefault();

        this.setState({ loadingJira: true, loading: true });

        fetch('api/Jira/worklog?selectedWeek=' + this.state.selectedWeek)
            .then(response => {
                if (!response.ok) {
                    (response.text() as Promise<String>).then(data => {
                        this.setState({ loadingJira: false, loadedJira: false, loading: false, popup: true, popup_error: true, popup_data: [data] });
                    });
                } else
                    (response.json() as Promise<WeekJiraIssuesResponse>).then(data => {
                        if (data.weekJiraIssues.length == 0) {
                            this.setState({ loadingJira: false, loadedJira: false, loading: false, popup: true, popup_error: true, popup_data: ["No hay tareas en Jira"] });

                        }
                        else {
                            fetch('api/Si3/AvailableHours?selectedWeek=' + this.state.selectedWeek).then(response => {
                                if (!response.ok) {
                                    (response.text() as Promise<String>).then(
                                        data => {
                                            this.setState({ popup: true, popup_error: true, popup_data: [data] });
                                        }
                                    );
                                } else {
                                    (response.json() as Promise<number>).then(data => {
                                        this.setState({ availableHours: 40 - data, loading: false, loadedJira: true });
                                        this.isDisabledBtnSi3();
                                    });
                                }
                            });

                            this.setState({ Weekissues: data.weekJiraIssues });
                        }
                    })
            })
            .catch(error => {
                alert(error);
                this.setState({ loadingJira: false, loadedJira: false, loading: false });
            });


    }
    private isDisabledBtnSi3() {

        let total = 0;
        let tiempo: number;
        let errores = 0;

        let WeekJiraIssues = this.state.Weekissues;
        for (let weekIssue of WeekJiraIssues) {
            for (let Issue of weekIssue.issues) {
                tiempo = Number(Issue.tiempo);
                total += tiempo;
                if ((tiempo % 1 != 0) || (Issue.issueSI3Code == null && Issue.tiempo > 0)) {
                    this.setState({ todoOk: true });
                    errores += 1;
                }
            }
        }
        if (errores == 0) {
            if (total <= this.state.availableHours) {
                this.setState({ todoOk: false });
            }
            else {
                this.setState({ todoOk: true });
            }
        }
    }
    //función para sacar las cookies, cname => userJira, passJira ... etc.
    public getCookie(cname: String) {
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

    private onLoginSi3(e: { preventDefault: () => void; }) {
        e.preventDefault();
        // si el radio button está seleccionado o no
        var submit = (this.refs["submitRadioBtn"] as HTMLInputElement).checked;

        this.setState({ loading: true });

        let agenda = (this.refs["agenda1"] as React.Component<WeekJiraIssuesProps, AgendaState>);
        let total = 0;
        let WeekJiraIssues = agenda.state.weekissues;
        for (let weekIssue of WeekJiraIssues) {
            for (let Issue of weekIssue.issues) {
                total += Number(Issue.tiempo);
            }
        }

        fetch('api/Si3/AvailableHours?selectedWeek=' + this.state.selectedWeek).then(response => {
            if (!response.ok) {
                (response.text() as Promise<String>).then(
                    data => {
                        this.setState({ popup: true, popup_error: true, popup_data: [data] });
                    }
                );
            } else {
                (response.json() as Promise<number>).then(data => {
                    if ((40 - data) != this.state.availableHours) {
                        this.setState({ availableHours: 40 - data, loading: false, popup: true, popup_error: true, popup_data: ["Se han imputador horas en Si3 mientras utilizabas eaSI3"] });
                    } else {
                        fetch('api/SI3/register?selectedWeek=' + this.state.selectedWeek + '&totalHours=' + total + '&submit=' + submit, {
                            method: 'post',
                            body: JSON.stringify(agenda.props.weekissues),
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json'
                            },
                        })
                            .then(response => {
                                if (!response.ok) {
                                    (response.json() as Promise<string[]>).then(data =>
                                    {
                                        this.setState({ loading: false, popup: true, popup_error: true, popup_data: data });
                                    });
                                } else {
                                    this.setState({ loading: false, todoOk: true, popup:true, popup_error: false, popup_data:["Horas imputadas correctamente"] });                                                                     
                                }
                            });
                    }
                });
            }
        });


    }


    public isTodoOk(val: boolean) { this.setState({ todoOk: val }); }
    public closePopup() {
        this.setState({ popup: false });
    }

    public render() {
        let agenda;
        let si3;
        let jira;
        let calendar;

        if (this.state.loadedJira) {
            jira = <input type="button" className="btnJira" value="Recargar" onClick={this.onLoginJira} />

            calendar = <div className="select-calendar">
                <label className="ocultoo">Elija semana de trabajo :</label>
                <select className="custom-select-ocultoo" /*onChange={this.handleChangeWeek}*/>
                    {
                        this.state.calendar.weeks.map(week =>
                            <option value={week.numberWeek} key={week.numberWeek} selected={week.numberWeek == this.state.calendar.weeks.length ? true : false}>
                                {week.description}
                            </option>)
                    }
                </select>
            </div>
        }

        if (this.state.loadedJira) {

            agenda = <Agenda weekissues={this.state.Weekissues} ref="agenda1" isTodoOk={this.isTodoOk} availableHours={this.state.availableHours} />
            si3 = <div className="container-si3">
                <input id="radiobtn" className="form-check-input" type="radio" ref="submitRadioBtn" value="option1" />
                <label className="form-check-label">
                    Submit en Si3
                </label>
                <br></br>
                <input type="button" className="btnSi3" value="Enviar a Si3" disabled={this.state.todoOk} onClick={this.onLoginSi3} /></div>;

        }

        return (
            <div>
                <span className="ocultoo">{this.state.calendar.version}</span>
                {calendar}
                <div className="container-home">
                {jira}
                {agenda}
                {si3}
                </div>
                {this.state.popup ?
                    <Popup error={this.state.popup_error} closePopup={this.closePopup} data={this.state.popup_data} /> : null
                }
                <Loader show={this.state.loading} message={<Cube/>} hideContentOnLoad={false} className={(this.state.loading == true) ? "overlay" : "overlay-1"} />
            </div>
        )
    }
}
export default Home


