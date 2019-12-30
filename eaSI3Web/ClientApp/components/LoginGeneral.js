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
import 'bootstrap';
import * as React from 'react';
import Loader from 'react-loader-advanced';
import ReactLoading from "react-loading";
import '../css/logingeneral.css';
import { Cube } from './Cube';
var LoginGeneral = /** @class */ (function (_super) {
    __extends(LoginGeneral, _super);
    function LoginGeneral(props) {
        var _this = _super.call(this, props) || this;
        _this.handleSubmit = _this.handleSubmit.bind(_this);
        _this.metodo = _this.metodo.bind(_this);
        _this.ValidateLogin = _this.ValidateLogin.bind(_this);
        _this.state = { users: [], userJiraLoaded: false, userSi3Loaded: false, loading: false };
        return _this;
    }
    LoginGeneral.prototype.componentDidMount = function () {
        this.metodo();
    };
    LoginGeneral.prototype.metodo = function () {
        var _this = this;
        fetch('api/Si3/users')
            .then(function (response) { return response.json(); })
            .then(function (data) {
            _this.setState({ users: data });
        });
    };
    LoginGeneral.prototype.handleSubmit = function (e) {
        var _this = this;
        e.preventDefault();
        var userJira = this.refs["tbUserJira"].value;
        var passJira = this.refs["tbPassJira"].value;
        var userSi3 = this.refs["tbUserSi3"].value;
        var passSi3 = this.refs["tbPassSi3"].value;
        this.setState({ loading: true });
        var urlJira = 'api/Jira/Login?';
        var urlSi3 = 'api/Si3/Login?';
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
                _this.setState({ userJiraLoaded: true });
            }
        })
            .then(function (data) {
            fetch(urlSi3, {
                method: 'post',
                body: JSON.stringify({ usernameJira: userJira, passwordJira: passJira, usernameSi3: userSi3, passwordSi3: passSi3 }),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
            })
                .then(function (response) {
                if (!response.ok) {
                    response.text().then(function (data) { alert(data); _this.setState({ userSi3Loaded: false, loading: false }); });
                }
                else {
                    response.json().then(function (data) {
                        _this.setState({ userSi3Loaded: true });
                        _this.ValidateLogin(data);
                    });
                }
            });
        });
    };
    LoginGeneral.prototype.ValidateLogin = function (idUser) {
        var codUserSi3 = this.refs["tbCodUserSi3"].value;
        var name = (this.refs["tbCodUserSi3"].children.item(this.refs["tbCodUserSi3"].selectedIndex)).innerText;
        if (this.state.userJiraLoaded && this.state.userSi3Loaded && codUserSi3 != "default") {
            fetch('api/Si3/SetCodUser', {
                method: 'post',
                body: JSON.stringify({ CodUser: codUserSi3, User: idUser, Name: name }),
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
            });
            this.setState({ loading: false });
            var expiration_date = new Date();
            expiration_date.setFullYear(expiration_date.getFullYear() + 1);
            document.cookie = "userId=" + idUser + "; path=/;" + "expires=" + expiration_date;
            document.cookie = "codUserSi3=" + codUserSi3 + "; path=/;" + "expires=" + expiration_date;
            this.props.onLogin(name);
        }
        else {
            this.setState({ loading: false });
        }
    };
    LoginGeneral.prototype.render = function () {
        var spinner = React.createElement("span", null,
            React.createElement(ReactLoading, { color: '#fff', type: 'spin', className: "spinner", height: 128, width: 128 }));
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
                        React.createElement("select", { ref: "tbCodUserSi3" },
                            React.createElement("option", { value: "default" }, "Seleccione un usuario"),
                            this.state.users.map(function (user) {
                                return React.createElement("option", { key: user.codigo, value: user.codigo }, user.nombre);
                            }))),
                    React.createElement("div", null,
                        React.createElement("input", { type: "text", name: "name", ref: "tbUserSi3", placeholder: "Introduzca usuario de Si3", autoComplete: "off" })),
                    React.createElement("div", null,
                        React.createElement("input", { type: "password", name: "name", ref: "tbPassSi3", placeholder: "Introduzca contrase\u00F1a de Si3", autoComplete: "off" })),
                    React.createElement("div", null,
                        React.createElement("input", { type: "submit", name: "submit", value: "Iniciar Sesi\u00F3n", className: "btn btn-primary" })))),
            React.createElement(Loader, { show: this.state.loading, message: React.createElement(Cube, null), hideContentOnLoad: false, className: (this.state.loading == true) ? "overlay" : "overlay-1" })));
    };
    return LoginGeneral;
}(React.Component));
export { LoginGeneral };
//# sourceMappingURL=LoginGeneral.js.map