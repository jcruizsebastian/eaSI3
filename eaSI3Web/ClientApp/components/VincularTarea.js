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
import * as React from "react";
import Loader from "react-loader-advanced";
import '../css/vincularTarea.css';
import { Cube } from "./Cube";
import { Popup } from "./Popup";
import * as ReactDOM from "react-dom";
var VincularTarea = /** @class */ (function (_super) {
    __extends(VincularTarea, _super);
    function VincularTarea(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            products: [], productSelected: "", componentSelected: "", moduleSelected: "", loadedData: false,
            loadedDataJira: false, titulo: "", prioridad: "", tipo: "", loading: false, responsable: "", todoOk: false, todoOkProject: false,
            projects: [], milestones: [], projectSelected: "", milestoneSelected: "", idSi3: "", popup: false, popup_error: false, popup_data: [], types: []
        };
        _this.focusLink = _this.focusLink.bind(_this);
        _this.focusLinkPro = _this.focusLinkPro.bind(_this);
        _this.closePopup = _this.closePopup.bind(_this);
        _this.handleSubmit = _this.handleSubmit.bind(_this);
        _this.renderInformación = _this.renderInformación.bind(_this);
        _this.handleChangeProducts = _this.handleChangeProducts.bind(_this);
        _this.handleChangeComponents = _this.handleChangeComponents.bind(_this);
        _this.handleChangeModules = _this.handleChangeModules.bind(_this);
        _this.handleChangeProject = _this.handleChangeProject.bind(_this);
        _this.handleChangeMilestone = _this.handleChangeMilestone.bind(_this);
        _this.handleChangeTypes = _this.handleChangeTypes.bind(_this);
        _this.vincular = _this.vincular.bind(_this);
        _this.vincularProyecto = _this.vincularProyecto.bind(_this);
        _this.generarInformacion = _this.generarInformacion.bind(_this);
        _this.mapType = _this.mapType.bind(_this);
        return _this;
    }
    VincularTarea.prototype.handleSubmit = function (e) {
        e.preventDefault();
        this.generarInformacion();
    };
    VincularTarea.prototype.mapType = function (type) {
        var tipo = "";
        switch (type) {
            case "Mantenimiento":
                tipo = "Data_Maintenance";
                break;
            case "Asistencia":
                tipo = "Asistencia";
                break;
            case "Bolsa de Horas":
                tipo = "Bolsa_de_horas";
                break;
            case "Corrección":
                tipo = "Defecto";
                break;
            case "Especificación":
            case "Análisis":
                tipo = "Especificacion";
                break;
            case "Formación":
            case "Microinformática":
                tipo = "Help_and_Documentation";
                break;
            case "Gestión":
            case "Vacaciones":
                tipo = "Gestion";
                break;
            case "Desarrollo":
            case "Tarea":
            case "Workpack":
            case "Calidad":
            case "Change Request":
                tipo = "Mejora";
                break;
            case "Preventa":
                tipo = "Pdte_asignacion_proyecto";
                break;
            case "Pruebas":
                tipo = "Pruebas";
                break;
            case "Sistemas":
            case "Interno":
                tipo = "Security";
                break;
            case "Épica":
            case "Permisos":
                tipo = "Mejora";
                break;
            default:
                tipo = "Mejora";
                break;
        }
        return tipo;
    };
    VincularTarea.prototype.generarInformacion = function () {
        var _this = this;
        var keyJira;
        if (this.props.jiraKey.length > 0) {
            keyJira = this.props.jiraKey;
        }
        else {
            keyJira = this.refs["tbKeyJira"].value;
        }
        this.setState({ loading: true });
        fetch('api/Si3/getTypes')
            .then(function (response) {
            if (!response.ok) {
                response.text().then(function (data) {
                    _this.setState({ loadedDataJira: false, loading: false, popup: true, popup_error: true, popup_data: [data] });
                });
            }
            else {
                response.json().then(function (data) {
                    _this.setState({ types: data });
                });
            }
        });
        fetch('api/Si3/products')
            .then(function (response) {
            if (!response.ok) {
                response.text().then(function (data) {
                    _this.setState({ loadedDataJira: false, loading: false, popup: true, popup_error: true, popup_data: [data] });
                });
            }
            else {
                response.json().then(function (data) {
                    _this.setState({ products: data, loadedData: true });
                    fetch('api/Jira/issue?jiraKey=' + keyJira)
                        .then(function (response) {
                        if (!response.ok) {
                            response.text().then(function (data) {
                                _this.setState({ loadedDataJira: false, loading: false, popup: true, popup_error: true, popup_data: [data] });
                            });
                        }
                        else {
                            response.json().then(function (data) {
                                if (data.si3ID == null || data.si3ID.charAt(data.si3ID.length - 1) == ";") {
                                    var prioridad_ = "";
                                    switch (data.priority) {
                                        case 1:
                                            prioridad_ = "Trivial";
                                            break;
                                        case 2:
                                            prioridad_ = "Menor";
                                            break;
                                        case 3:
                                            prioridad_ = "Mayor";
                                            break;
                                        case 4:
                                            prioridad_ = "Crítica";
                                            break;
                                        case 5:
                                            prioridad_ = "Bloqueadora";
                                            break;
                                    }
                                    var tipo_ = _this.mapType(data.issuetype);
                                    fetch('api/Si3/Projects')
                                        .then(function (response) {
                                        if (!response.ok) {
                                            response.text().then(function (data) {
                                                _this.setState({ loadedDataJira: false, loading: false, popup: true, popup_error: true, popup_data: [data] });
                                            });
                                        }
                                        else {
                                            response.json().then(function (data) {
                                                _this.setState({ projects: data });
                                                fetch('api/Si3/Milestones')
                                                    .then(function (response) {
                                                    if (!response.ok) {
                                                        response.text().then(function (data) {
                                                            _this.setState({ loadedDataJira: false, loading: false, popup: true, popup_error: true, popup_data: [data] });
                                                        });
                                                    }
                                                    else {
                                                        response.json().then(function (data) {
                                                            _this.setState({ milestones: data, loading: false, loadedDataJira: true });
                                                        });
                                                    }
                                                });
                                            });
                                        }
                                    });
                                    _this.setState({
                                        titulo: data.summary, prioridad: prioridad_,
                                        tipo: tipo_, responsable: data.assignee, idSi3: data.si3ID
                                    });
                                }
                                else {
                                    _this.setState({ loadedDataJira: false, loading: false, popup: true, popup_error: true, popup_data: ["Esta tarea ya está vinculada en SI3"] });
                                }
                            });
                        }
                    });
                });
            }
        });
    };
    VincularTarea.prototype.handleChangeProject = function (event) {
        this.setState({ projectSelected: event.currentTarget.value, milestoneSelected: "default", todoOkProject: false });
    };
    VincularTarea.prototype.handleChangeMilestone = function (event) {
        if (event.currentTarget.value == "default") {
            this.setState({ milestoneSelected: event.currentTarget.value, todoOkProject: false });
        }
        else
            this.setState({ milestoneSelected: event.currentTarget.value, todoOkProject: true });
    };
    VincularTarea.prototype.handleChangeProducts = function (event) {
        this.setState({ productSelected: event.currentTarget.value, componentSelected: "default", moduleSelected: "default", todoOk: false });
    };
    VincularTarea.prototype.handleChangeComponents = function (event) {
        if (event.currentTarget.value == "default") {
            this.setState({ componentSelected: event.currentTarget.value, moduleSelected: "default", todoOk: false });
        }
        else
            this.setState({ componentSelected: event.currentTarget.value, moduleSelected: "default", todoOk: true });
    };
    VincularTarea.prototype.handleChangeModules = function (event) {
        this.setState({ moduleSelected: event.currentTarget.value });
    };
    VincularTarea.prototype.handleChangeTypes = function (event) {
        this.setState({ tipo: event.currentTarget.value });
    };
    VincularTarea.prototype.vincularProyecto = function () {
        var _this = this;
        this.setState({ loading: true });
        var key;
        if (this.props.jiraKey.length > 0) {
            key = this.props.jiraKey;
        }
        else {
            key = this.refs["tbKeyJira"].value;
        }
        var code = this.state.milestoneSelected.replace("#", "-");
        fetch('api/Jira/updateIssueSi3Project?codeProject=' + this.state.projectSelected + '&codeMilestone=' + code + '&jiraKey=' + key + '&idSi3=' + this.state.idSi3)
            .then(function (response) {
            if (!response.ok) {
                response.text().then(function (data) {
                    _this.setState({ loading: false, popup: true, popup_error: true, popup_data: [data] });
                });
            }
            else {
                response.text().then(function (data) {
                    if (_this.props.jiraKey.length > 0) {
                        _this.props.vincular(data, key);
                    }
                    _this.setState({ loading: false, popup: true, popup_error: false, popup_data: ["Tarea vinculada"] });
                });
            }
        });
    };
    VincularTarea.prototype.vincular = function () {
        var _this = this;
        this.setState({ loading: true });
        var cod = this.getCookie("codUserSi3");
        var key;
        if (this.props.jiraKey.length > 0) {
            key = this.props.jiraKey;
        }
        else {
            key = this.refs["tbKeyJira"].value;
        }
        fetch('api/Si3/Linkissue', {
            method: 'post',
            body: JSON.stringify({
                JiraKey: key, Titulo: this.state.titulo, Prioridad: this.state.prioridad,
                Tipo: this.state.tipo, Producto: this.state.productSelected, Componente: this.state.componentSelected, Modulo: this.state.moduleSelected,
                Responsable: this.state.responsable, CodUserSi3: cod
            }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(function (response) {
            if (!response.ok) {
                response.text().then(function (data) {
                    _this.setState({ loading: false, popup: true, popup_error: true, popup_data: [data] });
                });
            }
            else {
                response.text().then(function (data) {
                    var issueKey = data.split("\"")[1];
                    if (_this.state.idSi3 != null) {
                        issueKey = _this.state.idSi3 + issueKey;
                    }
                    fetch('api/Jira/updateissuesi3customfield?issueKey=' + issueKey + '&jirakey=' + key)
                        .then(function (response) {
                        if (!response.ok) {
                            response.text().then(function (data) {
                                _this.setState({ loading: false, popup: true, popup_error: true, popup_data: [data] });
                            });
                        }
                        else {
                            if (_this.props.jiraKey.length > 0) {
                                _this.props.vincular(issueKey, key);
                            }
                            _this.setState({ loading: false, popup: true, popup_error: false, popup_data: ["Tarea Vinculada"] });
                        }
                    });
                });
            }
        });
    };
    //función para sacar las cookies, cname => userJira, passJira ... etc.
    VincularTarea.prototype.getCookie = function (cname) {
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
    VincularTarea.prototype.focusLink = function (e) {
        e.preventDefault();
        var link = ReactDOM.findDOMNode(this).querySelector(".btn-tarea");
        link.style.border = "red solid 3px";
        link.style.color = "red";
        var link = ReactDOM.findDOMNode(this).querySelector(".btn-proyecto");
        link.style.borderColor = "rgb(0,51,102)";
        link.style.color = "rgb(0,51,102)";
    };
    VincularTarea.prototype.focusLinkPro = function (e) {
        e.preventDefault();
        var link = ReactDOM.findDOMNode(this).querySelector(".btn-proyecto");
        link.style.border = "red solid 3px";
        link.style.color = "red";
        var link = ReactDOM.findDOMNode(this).querySelector(".btn-tarea");
        link.style.borderColor = "rgb(0,51,102)";
        link.style.color = "rgb(0,51,102)";
    };
    VincularTarea.prototype.renderInformación = function () {
        var _this = this;
        console.log(this.state.tipo);
        return (React.createElement("div", { style: {
                border: "4px solid white", padding: "20px", width: "600px", backgroundColor: "rgba(255,255,255,0.5)", margin: "auto"
            } },
            React.createElement("div", { style: { textAlign: "center" } },
                React.createElement("label", null,
                    React.createElement("p", { className: "ptext" }, "Vincular a : ")),
                React.createElement("div", { id: "tab", className: "btn-group", "data-toggle": "buttons-radio", style: { zIndex: 0 } },
                    React.createElement("a", { href: "#tarea", className: "btn-tarea active", "data-toggle": "tab", onClick: this.focusLink }, "Tarea"),
                    React.createElement("a", { href: "#proyecto", className: "btn-proyecto", "data-toggle": "tab", onClick: this.focusLinkPro }, "Proyecto"))),
            React.createElement("div", { className: "tab-content" },
                React.createElement("div", { className: "tab-pane active", id: "tarea" },
                    React.createElement("hr", null),
                    React.createElement("p", { className: "ptext" },
                        React.createElement("b", null, "T\u00EDtulo Jira :"),
                        " ",
                        this.state.titulo,
                        " "),
                    React.createElement("p", { className: "ptext" },
                        React.createElement("b", null, "Prioridad :"),
                        " ",
                        this.state.prioridad),
                    React.createElement("p", { className: "ptext" },
                        React.createElement("b", null, "Responsable : "),
                        this.state.responsable),
                    React.createElement("label", { className: "ptext" },
                        React.createElement("b", null, "Tipo : ")),
                    React.createElement("select", { className: "custom-select", onChange: this.handleChangeTypes }, this.state.types.map(function (type) {
                        return React.createElement("option", { key: type.cod, value: type.name, selected: type.name == _this.state.tipo }, type.name);
                    })),
                    React.createElement("br", null),
                    React.createElement("label", { className: "ptext" }, "Producto : "),
                    React.createElement("select", { className: "custom-select", onChange: this.handleChangeProducts },
                        React.createElement("option", { value: "default", selected: true }, "Seleccione un producto"),
                        this.state.products.map(function (product) {
                            return React.createElement("option", { key: product.code, value: product.code }, product.name);
                        })),
                    React.createElement("br", null),
                    React.createElement("label", { className: "ptext" }, "Componente : "),
                    React.createElement("select", { className: "custom-select", onChange: this.handleChangeComponents },
                        React.createElement("option", { value: "default", selected: true }, "Seleccione un componente"),
                        this.state.products.filter(function (product) { return product.code == _this.state.productSelected; }).map(function (p) { return p.componentes.map(function (component) {
                            return React.createElement("option", { key: component.code, value: component.code }, component.name);
                        }); })),
                    React.createElement("br", null),
                    React.createElement("label", { className: "ptext" }, "M\u00F3dulo : "),
                    React.createElement("select", { className: "custom-select", onChange: this.handleChangeModules },
                        React.createElement("option", { value: "default", selected: true }, "Seleccione un m\u00F3dulo"),
                        this.state.products.filter(function (product) { return product.code == _this.state.productSelected; }).map(function (p) { return p.componentes.filter(function (component) { return component.code == _this.state.componentSelected; }).map(function (c) { return c.modulos.map(function (modulo) {
                            return React.createElement("option", { key: modulo.code, value: modulo.code }, modulo.name);
                        }); }); })),
                    React.createElement("hr", null),
                    React.createElement("img", { src: "logo_open.png", width: "150", height: "auto", className: "img-open" }),
                    React.createElement("input", { type: "button", value: "Vincular", className: "btn-vincular", onClick: this.vincular, disabled: !this.state.todoOk })),
                React.createElement("div", { className: "tab-pane", id: "proyecto" },
                    React.createElement("hr", null),
                    React.createElement("label", { className: "ptext" }, "Proyecto : "),
                    React.createElement("select", { className: "custom-select", onChange: this.handleChangeProject },
                        React.createElement("option", { value: "default", selected: true }, "Seleccione un proyecto"),
                        this.state.projects.map(function (project) {
                            return React.createElement("option", { key: project.code, value: project.code }, project.title);
                        })),
                    React.createElement("br", null),
                    React.createElement("label", { className: "ptext" }, "Hito : "),
                    React.createElement("select", { className: "custom-select", onChange: this.handleChangeMilestone },
                        React.createElement("option", { value: "default", selected: true }, "Seleccione un hito"),
                        this.state.milestones.filter(function (milestone) { return milestone.projectCode.toString() == _this.state.projectSelected; }).map(function (m) { return m.milestone.map(function (milestone) {
                            return React.createElement("option", { key: milestone.name, value: milestone.name }, milestone.name);
                        }); })),
                    React.createElement("hr", null),
                    React.createElement("img", { src: "logo_open.png", width: "150", height: "auto", className: "img-open" }),
                    React.createElement("input", { type: "button", value: "Vincular", className: "btn-vincular", onClick: this.vincularProyecto, disabled: !this.state.todoOkProject })))));
    };
    VincularTarea.prototype.closePopup = function () {
        this.setState({ popup: false });
    };
    VincularTarea.prototype.render = function () {
        var formulario;
        if (this.props.jiraKey.length == 0) {
            formulario = React.createElement("div", { className: "container-vincular" },
                React.createElement("form", { className: "dataForm", onSubmit: this.handleSubmit },
                    React.createElement("img", { className: "main-about-jira-logo", src: "atlassian-jira-logo-large.png", width: "256", id: "img-jira" }),
                    React.createElement("input", { type: "text", id: "keyJira", className: "form-control", name: "key", ref: "tbKeyJira", placeholder: "Introduzca Key Jira", autoComplete: "off" }),
                    React.createElement("input", { type: "submit", className: "btnVincular", value: "Generar informaci\u00F3n" })),
                React.createElement("br", null));
        }
        if (this.props.jiraKey.length > 0 && !this.state.loading && !this.state.loadedData && !this.state.loadedDataJira) {
            formulario = React.createElement("div", null);
            this.generarInformacion();
        }
        var informacion;
        if (this.state.loadedData && this.state.loadedDataJira) {
            informacion = this.renderInformación();
            //this.focusLink();
        }
        return (React.createElement("div", null,
            formulario,
            React.createElement("div", { className: "container-vincular-informacion" }, informacion),
            this.state.popup ?
                React.createElement(Popup, { error: this.state.popup_error, closePopup: this.closePopup, data: this.state.popup_data }) : React.createElement("div", null),
            React.createElement(Loader, { show: this.state.loading, message: React.createElement(Cube, null), hideContentOnLoad: false, className: (this.state.loading == true) ? "overlay" : "overlay-1" })));
    };
    return VincularTarea;
}(React.Component));
export { VincularTarea };
//# sourceMappingURL=VincularTarea.js.map