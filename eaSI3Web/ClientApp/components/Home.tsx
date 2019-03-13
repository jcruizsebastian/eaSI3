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

interface WeekJiraIssuesResponse {
    weekJiraIssues: WeekJiraIssues[];
}

export class Home extends React.Component<{}, UserCredentialsState> {

    constructor(props: {}) {
        super(props);

        this.onLoginJira = this.onLoginJira.bind(this);
        this.onLoginSi3 = this.onLoginSi3.bind(this);
        this.confirmLoadedJira = this.confirmLoadedJira.bind(this);
        this.isTodoOk = this.isTodoOk.bind(this);
        this.getWeekofYear = this.getWeekofYear.bind(this);
        this.handleChangeWeek = this.handleChangeWeek.bind(this);
        this.getCookie = this.getCookie.bind(this);

        this.state = {
            Weekissues: [], loadedJira: false, loadingJira: false, calendar: { version: "", weeks: [] }, calendarLoaded: false, todoOk: false,
            loading: false, availableHours: 0
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
            });
    }

    private onLoginJira(e: { preventDefault: () => void; }) {
        e.preventDefault();

        this.setState({ loadingJira: true, loading: true });

        fetch('api/Jira/worklog?selectedWeek=' + this.state.selectedWeek)
            .then(response => {
                if (!response.ok) {
                    (response.text() as Promise<String>).then(data => {
                        alert(data);
                        this.setState({ loadingJira: false, loadedJira: false, loading: false });
                    });
                } else
                    (response.json() as Promise<WeekJiraIssuesResponse>).then(data => {
                        if (data.weekJiraIssues.length == 0) {
                            alert("No hay tareas en Jira");
                            this.setState({ loadingJira: false, loadedJira: false, loading: false });

                        }
                        else {
                            fetch('api/Si3/AvailableHours').then(response => {
                                if (!response.ok) {
                                    (response.text() as Promise<String>).then(
                                        data => {
                                            alert(data);
                                        }
                                    );
                                } else {
                                    (response.json() as Promise<number>).then(data => {
                                        this.setState({ availableHours: 40 - data, loading: false });
                                    });
                                }
                            });

                            this.setState({ Weekissues: data.weekJiraIssues }, this.confirmLoadedJira);
                        }
                    })
            })
            .catch(error => {
                alert(error);
                this.setState({ loadingJira: false, loadedJira: false, loading: false });
            });


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
        var submit = (this.refs["radioBtn"] as HTMLInputElement).checked;

        this.setState({ loading: true });
        let agenda = (this.refs["agenda1"] as React.Component<WeekJiraIssuesProps, AgendaState>);
        let total = 0;
        let WeekJiraIssues = agenda.state.weekissues;
        for (let weekIssue of WeekJiraIssues) {
            for (let Issue of weekIssue.issues) {
                total += Number(Issue.tiempo);
            }
        }

        fetch('api/Si3/AvailableHours').then(response => {
            if (!response.ok) {
                (response.text() as Promise<String>).then(
                    data => {
                        alert(data);
                    }
                );
            } else {
                (response.json() as Promise<number>).then(data => {
                    if ((40 - data) != this.state.availableHours) {
                        alert("Se han imputador horas en Si3 mientras utilizabas eaSI3");
                        this.setState({ availableHours: 40 - data, loading: false });
                    } else {
                        fetch('api/SI3/register?selectedWeek=' + this.state.selectedWeek + '&totalHours=' + total, {
                            method: 'post',
                            body: JSON.stringify(agenda.props.weekissues),
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json'
                            },
                        })
                            .then(response => {
                                if (!response.ok) {
                                    (response.text() as Promise<String>).then(data => { alert(data); this.setState({ loading: false }); });
                                } else {
                                    if (submit) {
                                        //hacer submit en Si3
                                        //fetch('api/Si3/Submit').then()
                                    }
                                    alert("Horas imputadas en SI3");
                                    this.setState({ loading: false, todoOk: true });
                                }
                            });
                    }
                });
            }
        });


    }

    public handleChangeWeek(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ selectedWeek: event.currentTarget.value });
    }

    public isTodoOk(val: boolean) { this.setState({ todoOk: val }); }

    public render() {
        let agenda;
        let si3;
        let jira;
        let calendar;

        if (this.state.calendarLoaded) {
            jira = <input type="button" value="Obtener issues" className="btn btn-primary" onClick={this.onLoginJira} />

            calendar = <div className="select-calendar">
                <label>Elija semana de trabajo :</label>
                <select className="custom-select" onChange={this.handleChangeWeek} >
                    {
                        this.state.calendar.weeks.map(week =>
                            <option value={week.numberWeek} key={week.numberWeek} selected={week.numberWeek == this.state.calendar.weeks.length ? true : false}>
                                {week.description}
                            </option>)
                    }
                </select>
            </div>
        }

        if (this.state.loadedJira && this.state.availableHours > 0) {

            agenda = <Agenda weekissues={this.state.Weekissues} ref="agenda1" isTodoOk={this.isTodoOk} availableHours={this.state.availableHours} />
            si3 = <div>
                <input id="radiobtn" className="form-check-input" type="radio" ref="submitRadioBtn" value="option1" disabled />
                <label className="form-check-label">
                    Submit en Si3
                </label>
                <br></br>
                <input type="button" id="btnSi3" value="Imputar tareas en Si3" className="btn btn-primary" disabled={this.state.todoOk} onClick={this.onLoginSi3} /></div>;

        }

        const spinner = <span><ReactLoading color='#fff' type='spin' className="spinner" height={128} width={128} /></span>

        return (
            <div>
                <span className="oculto">{this.state.calendar.version}</span>
                {calendar}
                {jira}
                {agenda}
                {si3}
                <Loader show={this.state.loading} message={spinner} hideContentOnLoad={false} className={(this.state.loading == true) ? "overlay" : "overlay-1"} />
            </div>
        )
    }
}
export default Home


