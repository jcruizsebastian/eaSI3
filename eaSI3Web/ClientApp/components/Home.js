var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
import 'isomorphic-fetch';
import * as React from 'react';
import Loader from 'react-loader-advanced';
import { Agenda } from './Agenda';
import { Cube } from './Cube';
import { Popup } from './Popup';
var Home = /** @class */ (function (_super) {
    __extends(Home, _super);
    function Home(props) {
        var _this = _super.call(this, props) || this;
        _this.closePopup = _this.closePopup.bind(_this);
        _this.onLoginJira = _this.onLoginJira.bind(_this);
        _this.onLoginSi3 = _this.onLoginSi3.bind(_this);
        _this.confirmLoadedJira = _this.confirmLoadedJira.bind(_this);
        _this.isTodoOk = _this.isTodoOk.bind(_this);
        _this.getWeekofYear = _this.getWeekofYear.bind(_this);
        _this.isDisabledBtnSi3 = _this.isDisabledBtnSi3.bind(_this);
        _this.getCookie = _this.getCookie.bind(_this);
        _this.handleChangeWeek = _this.handleChangeWeek.bind(_this);
        _this.state = {
            Weekissues: [], loadedJira: false, loadingJira: false, calendar: { version: "", weeks: [] }, calendarLoaded: false, todoOk: false,
            loading: false, availableHours: 0, popup: false, popup_error: false, popup_data: []
        };
        return _this;
    }
    Home.prototype.componentDidMount = function () {
        this.getWeekofYear();
    };
    Home.prototype.confirmLoadedJira = function () {
        this.setState({
            loadingJira: false,
            loadedJira: true
        });
    };
    Home.prototype.handleChangeWeek = function (event) {
        this.setState({ selectedWeek: event.currentTarget.value });
    };
    Home.prototype.getWeekofYear = function () {
        var _this = this;
        fetch('api/Jira/Weeks')
            .then(function (response) { return response.json(); })
            .then(function (data) {
            var weekNumber;
            data.weeks.forEach(function (week) {
                if (week.actualWeek == true) {
                    weekNumber = week.numberWeek.toString();
                }
            });
            _this.setState({ calendar: data, calendarLoaded: true, selectedWeek: weekNumber });
            _this.onLoginJira();
        });
    };
    Home.prototype.onLoginJira = function () {
        //e.preventDefault();
        var _this = this;
        this.setState({ loadingJira: true, loading: true });
        fetch('api/Jira/worklog?selectedWeek=' + this.state.selectedWeek)
            .then(function (response) {
            if (!response.ok) {
                response.text().then(function (data) {
                    _this.setState({ loadingJira: false, loadedJira: false, loading: false, popup: true, popup_error: true, popup_data: [data] });
                });
            }
            else
                response.json().then(function (data) {
                    // if (data.weekJiraIssues.length == 0) {
                    //      this.setState({ loadingJira: false, loadedJira: false, loading: false, popup: true, popup_error: true, popup_data: ["No hay tareas en Jira"] });
                    // }
                    // else {
                    fetch('api/Si3/AvailableHours?selectedWeek=' + _this.state.selectedWeek).then(function (response) {
                        if (!response.ok) {
                            response.text().then(function (data) {
                                _this.setState({ loadingJira: false, loading: false, popup: true, popup_error: true, popup_data: [data] });
                                _this.isDisabledBtnSi3();
                            });
                        }
                        else {
                            response.json().then(function (data) {
                                _this.setState({ availableHours: 40 - data, loading: false, loadedJira: true });
                                _this.isDisabledBtnSi3();
                            });
                        }
                    });
                    _this.setState({ Weekissues: data.weekJiraIssues });
                    // }
                });
        })
            .catch(function (error) {
            alert(error);
            _this.setState({ loadingJira: false, loadedJira: false, loading: false });
        });
    };
    Home.prototype.isDisabledBtnSi3 = function () {
        var total = 0;
        var tiempo;
        var errores = 0;
        var WeekJiraIssues = this.state.Weekissues;
        for (var _i = 0, WeekJiraIssues_1 = WeekJiraIssues; _i < WeekJiraIssues_1.length; _i++) {
            var weekIssue = WeekJiraIssues_1[_i];
            for (var _a = 0, _b = weekIssue.issues; _a < _b.length; _a++) {
                var Issue = _b[_a];
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
    };
    //función para sacar las cookies, cname => userJira, passJira ... etc.
    Home.prototype.getCookie = function (cname) {
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
    };
    Home.prototype.onLoginSi3 = function (e) {
        var _this = this;
        e.preventDefault();
        var mensaje = true;
        // si el radio button está seleccionado o no
        var submit = this.refs["submitRadioBtn"].checked;
        this.setState({ loading: true });
        var agenda = this.refs["agenda1"];
        var total = 0;
        var WeekJiraIssues = agenda.props.weekissues;
        for (var _i = 0, WeekJiraIssues_2 = WeekJiraIssues; _i < WeekJiraIssues_2.length; _i++) {
            var weekIssue = WeekJiraIssues_2[_i];
            for (var _a = 0, _b = weekIssue.issues; _a < _b.length; _a++) {
                var Issue = _b[_a];
                total += Number(Issue.tiempo);
            }
        }
        if (total < 40 && submit) {
            mensaje = confirm("¿Estas seguro de que quieres hacer submit de " + total + " horas?");
        }
        if (mensaje) {
            fetch('api/Si3/AvailableHours?selectedWeek=' + this.state.selectedWeek).then(function (response) {
                if (!response.ok) {
                    response.text().then(function (data) {
                        _this.setState({ loading: false, popup: true, popup_error: true, popup_data: [data] });
                    });
                }
                else {
                    response.json().then(function (data) {
                        if ((40 - data) != _this.state.availableHours) {
                            _this.setState({ availableHours: 40 - data, loading: false, popup: true, popup_error: true, popup_data: ["Se han imputador horas en Si3 mientras utilizabas eaSI3"] });
                        }
                        else {
                            fetch('api/SI3/register?selectedWeek=' + _this.state.selectedWeek + '&totalHours=' + total + '&submit=' + submit, {
                                method: 'post',
                                body: JSON.stringify(agenda.props.weekissues),
                                headers: {
                                    'Accept': 'application/json',
                                    'Content-Type': 'application/json'
                                },
                            }).then(function (response) {
                                if (!response.ok) {
                                    response.json().then(function (data) {
                                        _this.setState({ loading: false, popup: true, popup_error: true, popup_data: data });
                                    });
                                }
                                else {
                                    _this.setState({ loading: false, todoOk: true, popup: true, popup_error: false, popup_data: ["Horas imputadas correctamente"] });
                                }
                            });
                        }
                    });
                }
            });
        }
        else {
            this.setState({ loading: false });
        }
    };
    Home.prototype.isTodoOk = function (val) { this.setState({ todoOk: val }); };
    Home.prototype.closePopup = function () {
        this.setState({ popup: false });
    };
    Home.prototype.render = function () {
        var _this = this;
        var agenda;
        var si3;
        var jira;
        var calendar;
        if (this.state.loadedJira) {
            jira = React.createElement("input", { type: "button", className: "btnJira", value: "Recargar", onClick: this.onLoginJira });
            calendar = React.createElement("div", { className: "select-calendar" },
                React.createElement("label", { className: "ocultoo" }, "Elija semana de trabajo :"),
                React.createElement("select", { className: "custom-select-ocultoo", onChange: this.handleChangeWeek }, this.state.calendar.weeks.map(function (week) {
                    return React.createElement("option", { value: week.numberWeek, key: week.numberWeek, selected: week.numberWeek.toString() == _this.state.selectedWeek ? true : false }, week.description);
                })));
        }
        if (this.state.loadedJira) {
            agenda = React.createElement(Agenda, { weekissues: this.state.Weekissues, ref: "agenda1", isTodoOk: this.isTodoOk, availableHours: this.state.availableHours });
            si3 = React.createElement("div", { className: "container-si3" },
                React.createElement("input", { id: "checkbox", className: "form-check-input", type: "checkbox", ref: "submitRadioBtn", value: "option1" }),
                React.createElement("label", { className: "form-check-label" }, "Submit en Si3"),
                React.createElement("br", null),
                React.createElement("input", { type: "button", className: "btnSi3", value: "Enviar a Si3", disabled: this.state.todoOk, onClick: this.onLoginSi3 }));
        }
        return (React.createElement("div", null,
            React.createElement("span", { className: "oculto" }, this.state.calendar.version),
            calendar,
            React.createElement("div", { className: "container-home" },
                jira,
                agenda,
                si3),
            this.state.popup ?
                React.createElement(Popup, { error: this.state.popup_error, closePopup: this.closePopup, data: this.state.popup_data }) : null,
            React.createElement(Loader, { show: this.state.loading, message: React.createElement(Cube, null), hideContentOnLoad: false, className: (this.state.loading == true) ? "overlay" : "overlay-1" })));
    };
    return Home;
}(React.Component));
export { Home };
export default Home;
//# sourceMappingURL=Home.js.map