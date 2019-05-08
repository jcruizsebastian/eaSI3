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
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
import * as React from 'react';
import { LoginGeneral } from './LoginGeneral';
import { Link } from 'react-router-dom';
var Layout = /** @class */ (function (_super) {
    __extends(Layout, _super);
    function Layout(props) {
        var _this = _super.call(this, props) || this;
        _this.onLogin = _this.onLogin.bind(_this);
        _this.getCookie = _this.getCookie.bind(_this);
        _this.logout = _this.logout.bind(_this);
        _this.validate = _this.validate.bind(_this);
        _this.setCookie = _this.setCookie.bind(_this);
        _this.state = { logged: false, cookiesOk: false, name: "", loaded: false };
        return _this;
    }
    Layout.prototype.componentDidMount = function () {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.validate()];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    //funci�n para sacar las cookies, cname => userJira, passJira ... etc.
    Layout.prototype.getCookie = function (cname) {
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
    Layout.prototype.validate = function () {
        var _this = this;
        if (this.getCookie("codUserSi3") == "" || this.getCookie("userId") == "") {
            this.setCookie("_ga", "", 0);
            this.setCookie("userJira", "", 0);
            this.setCookie("passJira", "", 0);
            this.setCookie("userSi3", "", 0);
            this.setCookie("passSi3", "", 0);
            this.setCookie("codUserSi3", "", 0);
            this.setCookie("userId", "", 0);
        }
        this.setState({ loaded: false });
        fetch('api/Jira/ValidateLogin', {
            method: 'post',
            body: JSON.stringify({ username: "", password: "" }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(function (response) {
            if (!response.ok) {
                response.text().then(function (data) {
                    alert(data);
                    _this.setState({ loaded: true, cookiesOk: false });
                    _this.logout();
                });
            }
            else {
                fetch('api/Si3/ValidateLogin?', {
                    method: 'post',
                    body: JSON.stringify({ username: "", password: "" }),
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                })
                    .then(function (response) {
                    if (!response.ok) {
                        _this.setState({ cookiesOk: false, loaded: true });
                        _this.logout();
                    }
                    else {
                        _this.setState({ cookiesOk: true, loaded: true });
                    }
                });
            }
        });
    };
    //funci�n para cambiar una cookie
    Layout.prototype.setCookie = function (cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    };
    Layout.prototype.onLogin = function (nameUser) {
        localStorage.removeItem('name');
        localStorage.setItem("name", nameUser);
        this.setState({ logged: true, cookiesOk: true });
    };
    Layout.prototype.logout = function () {
        this.setCookie("userId", "", 0);
        this.setCookie("codUserSi3", "", 0);
        this.setState({ logged: false });
    };
    Layout.prototype.render = function () {
        var home;
        if (this.state.loaded) {
            if (this.getCookie("codUserSi3") == "" || this.getCookie("userId") == "") {
                home = React.createElement(LoginGeneral, { onLogin: this.onLogin });
            }
            else if (this.state.cookiesOk) {
                var name = localStorage.getItem('name');
                home = React.createElement("div", null,
                    React.createElement("div", { className: 'row' },
                        React.createElement("div", { className: 'col-sm-12' },
                            React.createElement(Link, { to: '/', style: { color: "white", marginLeft: "10px", marginBottom: "-20px" } },
                                React.createElement("span", { className: 'glyphicon glyphicon-chevron-left' }),
                                " Volver"),
                            React.createElement("input", { type: "button", className: "btn btn-secondary", id: "logout", value: "Log out", onClick: this.logout }),
                            React.createElement("label", { id: "name" }, name))),
                    React.createElement("div", { className: 'row' },
                        React.createElement("div", { className: 'container-fluid-navmenu' }, this.props.children)));
            }
            else {
                home = React.createElement(LoginGeneral, { onLogin: this.onLogin });
            }
        }
        return React.createElement("div", { className: 'container-fluid' }, home);
    };
    return Layout;
}(React.Component));
export { Layout };
//# sourceMappingURL=Layout.js.map