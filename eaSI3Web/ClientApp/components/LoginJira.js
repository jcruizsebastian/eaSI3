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
import * as React from 'react';
import Loader from 'react-loader-advanced';
import { Cube } from './Cube';
var LoginJira = /** @class */ (function (_super) {
    __extends(LoginJira, _super);
    function LoginJira(props) {
        var _this = _super.call(this, props) || this;
        _this.handleSubmit = _this.handleSubmit.bind(_this);
        _this.Login = _this.Login.bind(_this);
        _this.state = { loading: false, userJiraLoaded: false };
        return _this;
    }
    LoginJira.prototype.handleSubmit = function (e) {
        var _this = this;
        e.preventDefault();
        this.setState({ loading: true });
        var userJira = this.refs["tbUserJira"].value;
        var passJira = this.refs["tbPassJira"].value;
        var userSi3 = "";
        var passSi3 = "";
        var urlJira = 'api/Jira/Login?';
        fetch(urlJira, {
            method: 'post',
            body: JSON.stringify({ usernameJira: userJira, passwordJira: passJira, usernameSi3: userSi3, passwordSi3: passSi3 }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(function (response) {
            if (!response.ok) {
                response.text().then(function (data) { alert(data); _this.setState({ userJiraLoaded: false, loading: false }); });
            }
            else {
                _this.Login();
            }
        });
    };
    LoginJira.prototype.Login = function () {
        var _this = this;
        var name = "";
        fetch("/api/Jira/GetUserName").then(function (response) {
            if (response.ok) {
                response.text().then(function (data) {
                    name = data;
                    fetch("/api/Jira/GetCodUser").then(function (response) {
                        if (response.ok) {
                            response.json().then(function (data) {
                                var expiration_date = new Date();
                                expiration_date.setFullYear(expiration_date.getFullYear() + 1);
                                document.cookie = "codUserSi3=" + data + "; path=/;" + "expires=" + expiration_date;
                                _this.props.onLogin(name);
                            });
                        }
                    });
                });
            }
        });
    };
    LoginJira.prototype.render = function () {
        return (React.createElement("div", { className: "bodyLogin" },
            React.createElement("form", { className: "formulario", onSubmit: this.handleSubmit },
                React.createElement("div", { className: "form1" },
                    React.createElement("img", { src: "logo_open.png", width: "200", height: "75", className: "img" }),
                    React.createElement("div", null,
                        React.createElement("label", null, "Si accede a eaSi3 podr\u00E1 imputar sus horas de trabajo de una forma r\u00E1pida y sencilla."))),
                React.createElement("div", { className: "form2" },
                    React.createElement("div", null,
                        React.createElement("input", { type: "text", name: "name", ref: "tbUserJira", placeholder: "Introduzca usuario de Jira", autoComplete: "off" })),
                    React.createElement("div", null,
                        React.createElement("input", { type: "password", name: "name", ref: "tbPassJira", placeholder: "Introduzca contrase\u00F1a de Jira", autoComplete: "off" })),
                    React.createElement("div", null,
                        React.createElement("input", { type: "submit", name: "submit", value: "Iniciar Sesi\u00F3n", className: "btn btn-primary" })))),
            React.createElement(Loader, { show: this.state.loading, message: React.createElement(Cube, null), hideContentOnLoad: false, className: (this.state.loading == true) ? "overlay" : "overlay-1" })));
    };
    return LoginJira;
}(React.Component));
export { LoginJira };
//# sourceMappingURL=LoginJira.js.map