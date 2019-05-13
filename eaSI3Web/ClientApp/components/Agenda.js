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
import '../css/agenda.css';
import { PopupVincularTarea } from './popupVincularTarea';
import * as ReactDOM from 'react-dom';
var Agenda = /** @class */ (function (_super) {
    __extends(Agenda, _super);
    function Agenda(props) {
        var _this = _super.call(this, props) || this;
        _this.labels = {};
        _this.isDisabledBtnSi3 = _this.isDisabledBtnSi3.bind(_this);
        _this.timeReassignment = _this.timeReassignment.bind(_this);
        _this.calculateTotalHours = _this.calculateTotalHours.bind(_this);
        _this.vincular = _this.vincular.bind(_this);
        _this.closePopup = _this.closePopup.bind(_this);
        _this.checkBoxChanged = _this.checkBoxChanged.bind(_this);
        _this.state = { weekissues: _this.props.weekissues, link: "https://jira.openfinance.es/browse/", vincular: false, issueVincular: "", checked: false, expand: 0 };
        return _this;
    }
    Agenda.prototype.componentDidMount = function () {
        //con esto tengo todos los <label>
        console.log(ReactDOM.findDOMNode(this).querySelectorAll(".agenda-events-label"));
        var labels = ReactDOM.findDOMNode(this).querySelectorAll(".agenda-events-label");
        for (var i = 0; i < labels.length; i++) {
            var label = labels[i];
            var overflowX = label.offsetWidth < label.scrollWidth, overflowY = label.offsetHeight < label.scrollHeight;
            if (!overflowX && !overflowY) {
                label.className = "agenda-events-label-normal";
            }
        }
        var todoOk = this.isDisabledBtnSi3();
        this.props.isTodoOk(todoOk);
    };
    Agenda.prototype.timeReassignment = function (event) {
        var day = event.currentTarget.id.split('|')[1];
        var issueId = event.currentTarget.id.split('|')[0];
        for (var _i = 0, _a = this.props.weekissues; _i < _a.length; _i++) {
            var dayIssue = _a[_i];
            if (new Date(dayIssue.fecha.toString()).getDate().toString() == day) {
                for (var _b = 0, _c = dayIssue.issues; _b < _c.length; _b++) {
                    var issue = _c[_b];
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
    };
    Agenda.prototype.isDisabledBtnSi3 = function () {
        var total = 0;
        var tiempo;
        var WeekJiraIssues = this.props.weekissues;
        for (var _i = 0, WeekJiraIssues_1 = WeekJiraIssues; _i < WeekJiraIssues_1.length; _i++) {
            var weekIssue = WeekJiraIssues_1[_i];
            for (var _a = 0, _b = weekIssue.issues; _a < _b.length; _a++) {
                var Issue = _b[_a];
                tiempo = Number(Issue.tiempo);
                total += tiempo;
                if ((tiempo % 1 != 0) || (Issue.issueSI3Code == null && Issue.tiempo > 0)) {
                    return true;
                }
            }
        }
        if (total <= this.props.availableHours) {
            return false;
        }
        else {
            return true;
        }
    };
    Agenda.prototype.vincular = function (issuekey) {
        this.setState({ vincular: true, issueVincular: issuekey });
    };
    Agenda.prototype.calculateTotalHours = function () {
        var total = 0;
        var tiempo;
        var WeekJiraIssues = this.props.weekissues;
        for (var _i = 0, WeekJiraIssues_2 = WeekJiraIssues; _i < WeekJiraIssues_2.length; _i++) {
            var weekIssue = WeekJiraIssues_2[_i];
            for (var _a = 0, _b = weekIssue.issues; _a < _b.length; _a++) {
                var Issue = _b[_a];
                tiempo = Number(Issue.tiempo);
                total += tiempo;
            }
        }
        return total;
    };
    Agenda.prototype.closePopup = function (idSi3, key) {
        if (idSi3.length > 0) {
            var WeekJiraIssues = this.props.weekissues;
            for (var _i = 0, WeekJiraIssues_3 = WeekJiraIssues; _i < WeekJiraIssues_3.length; _i++) {
                var weekIssue = WeekJiraIssues_3[_i];
                for (var _a = 0, _b = weekIssue.issues; _a < _b.length; _a++) {
                    var Issue = _b[_a];
                    if (Issue.issueSI3Code == null && Issue.issueKey == key) {
                        Issue.issueSI3Code = idSi3;
                    }
                }
            }
        }
        var todoOk = this.isDisabledBtnSi3();
        this.props.isTodoOk(todoOk);
        this.setState({ vincular: false });
    };
    Agenda.prototype.checkBoxChanged = function (event) {
        this.setState({ checked: event.target.checked });
    };
    Agenda.prototype.render = function () {
        var _this = this;
        var i = 0;
        var total = this.calculateTotalHours();
        return React.createElement("div", { className: "table-container" },
            this.state.vincular ?
                React.createElement(PopupVincularTarea, { keyJira: this.state.issueVincular, closePopup: this.closePopup })
                : null,
            React.createElement("div", { className: "table-responsive" },
                React.createElement("table", { className: "table" },
                    React.createElement("thead", { className: "thead-dark" },
                        React.createElement("tr", null,
                            React.createElement("th", { className: "table-th-fecha" }, "Fecha"),
                            React.createElement("th", { className: "table-th" }, "Tarea"),
                            React.createElement("th", { className: "table-th-idSi3" }, "Id Si3"),
                            React.createElement("th", { className: "table-th-tipo" }, "Tipo de Tarea"),
                            React.createElement("th", { className: "table-th" }, "Tareas"),
                            React.createElement("th", { className: "table-th-horas" }, "Horas"))),
                    React.createElement("tbody", null,
                        this.props.weekissues.map(function (Weekissue) {
                            return React.createElement("tr", { key: Weekissue.fecha.toString() },
                                React.createElement("td", { className: "agenda-date" },
                                    React.createElement("div", { className: "shortdate text-dark" },
                                        " ",
                                        new Date(Weekissue.fecha.toString()).toLocaleDateString())),
                                React.createElement("td", { className: "agenda-events-td-id" },
                                    Weekissue.issues.filter(function (issue) { return issue.issueSI3Code == null; }).map(function (x) {
                                        return React.createElement("div", { className: "agenda-events-id", key: x.issueCode },
                                            React.createElement("a", { className: "agenda-events-id-link", target: "_blank", href: _this.state.link.concat(x.issueKey) },
                                                x.issueKey,
                                                React.createElement("span", { className: "tooltiptext-id" }, "Enlace a Jira")));
                                    }),
                                    Weekissue.issues.filter(function (issue) { return issue.issueSI3Code != null; }).map(function (issue) {
                                        return React.createElement("div", { className: "agenda-events-id", key: issue.issueCode },
                                            React.createElement("a", { className: "agenda-events-id-link", target: "_blank", href: _this.state.link.concat(issue.issueKey) },
                                                issue.issueKey,
                                                React.createElement("span", { className: "tooltiptext-id" }, "Enlace a Jira")));
                                    })),
                                React.createElement("td", { className: "agenda-events-td-idSi3" },
                                    Weekissue.issues.filter(function (issue) { return issue.issueSI3Code == null; }).map(function (x) {
                                        return React.createElement("div", { className: "agenda-events-idSi3", key: x.issueCode },
                                            React.createElement("button", { type: "button", id: "btn-vincular", className: "btn btn-danger btn-sm", onClick: function () { _this.vincular(x.issueKey); } }, "Vincular"));
                                    }),
                                    Weekissue.issues.filter(function (issue) { return issue.issueSI3Code != null; }).map(function (issue) {
                                        return React.createElement("div", { className: "agenda-events-idSi3", key: issue.issueCode },
                                            React.createElement("label", { className: "issue-si3" }, issue.issueSI3Code));
                                    })),
                                React.createElement("td", { className: "agenda-events-td-tipo" },
                                    Weekissue.issues.filter(function (issue) { return issue.issueSI3Code == null; }).map(function (x) {
                                        return React.createElement("div", { className: "agenda-events-tipo" },
                                            React.createElement("label", { className: "issue-tipo" }, x.tipo));
                                    }),
                                    Weekissue.issues.filter(function (issue) { return issue.issueSI3Code != null; }).map(function (issue) {
                                        return React.createElement("div", { className: "agenda-events-tipo" },
                                            React.createElement("label", { className: "issue-tipo" }, issue.tipo));
                                    })),
                                React.createElement("td", { className: "agenda-events-table-title" },
                                    Weekissue.issues.filter(function (issue) { return issue.issueSI3Code == null; }).map(function (issue) {
                                        return React.createElement("div", { className: "agenda-events-title", key: issue.titulo + issue.issueCode },
                                            React.createElement("label", { className: "agenda-events-label", title: issue.titulo },
                                                " ",
                                                issue.titulo));
                                    }),
                                    Weekissue.issues.filter(function (issue) { return issue.issueSI3Code != null; }).map(function (x) {
                                        return React.createElement("div", { className: "agenda-events-title", key: x.titulo + x.issueCode },
                                            React.createElement("label", { className: "agenda-events-label", title: x.titulo },
                                                " ",
                                                x.titulo));
                                    })),
                                React.createElement("td", { className: "agenda-events" },
                                    Weekissue.issues.filter(function (issue) { return issue.issueSI3Code == null; }).map(function (issue) {
                                        return React.createElement("div", { className: "agenda-events-hours", key: issue.issueKey },
                                            React.createElement("input", { type: "text", id: issue.issueKey + '|' + new Date(Weekissue.fecha.toString()).getDate(), name: "tbTiempoCorregido", value: issue.tiempo, placeholder: String(issue.tiempo), onChange: _this.timeReassignment, className: ((Number(issue.tiempo) % 1 != 0) || (issue.issueSI3Code == null && issue.tiempo > 0)) ? "invalid" : "valid", autoComplete: "off" }));
                                    }),
                                    Weekissue.issues.filter(function (issue) { return issue.issueSI3Code != null; }).map(function (issue) {
                                        return React.createElement("div", { className: "agenda-events-hours", key: issue.issueKey },
                                            React.createElement("input", { type: "text", id: issue.issueKey + '|' + new Date(Weekissue.fecha.toString()).getDate(), name: "tbTiempoCorregido", value: issue.tiempo, placeholder: String(issue.tiempo), onChange: _this.timeReassignment, className: ((Number(issue.tiempo) % 1 != 0) || (issue.issueSI3Code == null && issue.tiempo > 0)) ? "invalid" : "valid", autoComplete: "off" }));
                                    })));
                        }),
                        React.createElement("tr", null,
                            React.createElement("td", { className: "agenda-table-total-first" }),
                            React.createElement("td", { className: "agenda-table-total" }),
                            React.createElement("td", { className: "agenda-table-total" }),
                            React.createElement("td", { className: "agenda-table-total" }),
                            React.createElement("td", { className: "agenda-table-total" }),
                            React.createElement("td", { className: "agenda-table-total-last" },
                                React.createElement("label", { className: "agenda-total" },
                                    "Horas : ",
                                    total.toString(),
                                    "/",
                                    this.props.availableHours.toString(),
                                    " ")))))),
            React.createElement("br", null));
    };
    return Agenda;
}(React.Component));
export { Agenda };
export default Agenda;
//# sourceMappingURL=Agenda.js.map