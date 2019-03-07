import * as React from "react";
import Loader from "react-loader-advanced";
import ReactLoading from "react-loading";
import '../css/vincularTarea.css';
import { Product } from './Model/Product';
import { VincularState } from './Model/States/VincularState'
import { Issue } from './Model/Issue'
import { VincularTareaProps } from "./Model/Props/VincularTareaProps";
import { Project } from "./Model/Project";
import { Milestones } from "./Model/Milestones";

export class VincularTarea extends React.Component<VincularTareaProps, VincularState> {
    constructor(props: VincularTareaProps) {
        super(props);

        this.state = {
            products: [], productSelected: "", componentSelected: "", moduleSelected: "", loadedData: false,
            loadedDataJira: false, titulo: "", prioridad: "", tipo: "", loading: false, responsable: "", todoOk: false, todoOkProject: false,
            projects: [], milestones: [], projectSelected: "", milestoneSelected:""
        };

        this.handleSubmit = this.handleSubmit.bind(this);
        this.renderInformación = this.renderInformación.bind(this);
        this.handleChangeProducts = this.handleChangeProducts.bind(this);
        this.handleChangeComponents = this.handleChangeComponents.bind(this);
        this.handleChangeModules = this.handleChangeModules.bind(this);
        this.handleChangeProject = this.handleChangeProject.bind(this);
        this.handleChangeMilestone = this.handleChangeMilestone.bind(this);
        this.vincular = this.vincular.bind(this);
        this.vincularProyecto = this.vincularProyecto.bind(this);
        this.generarInformacion = this.generarInformacion.bind(this);
    }

    public handleSubmit(e: { preventDefault: () => void; }) {
        e.preventDefault();
        this.generarInformacion();

    }

    public generarInformacion() {
        var keyJira: string;

        if (this.props.jiraKey.length > 0) {
            keyJira = this.props.jiraKey;
        } else {
            keyJira = (this.refs["tbKeyJira"] as HTMLInputElement).value;
        }

        this.setState({ loading: true });

        fetch('api/Si3/products?username=' + this.getCookie("userSi3") + '&password=' + this.getCookie("passSi3"))
            .then(response => {
                if (!response.ok) {
                    (response.text() as Promise<String>).then(
                        data => {
                            alert(data);
                            this.setState({ loadedDataJira: false, loading: false });
                        }
                    );
                }
                else {
                    (response.json() as Promise<Product[]>).then(
                        data => {
                            this.setState({ products: data, loadedData: true });
                            fetch('api/Jira/issue?username=' + this.getCookie("userJira") + '&password=' + this.getCookie("passJira") + '&jiraKey=' + keyJira)
                                .then(response => {
                                    if (!response.ok) {
                                        (response.text() as Promise<String>).then(
                                            data => {
                                                alert(data);
                                                this.setState({ loadedDataJira: false, loading: false });
                                            }
                                        )
                                    }
                                    else {
                                        (response.json() as Promise<Issue>).then(
                                            data => {
                                                if (data.si3ID == null) {
                                                    var prioridad_: string = "";
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
                                                            break
                                                    }

                                                    fetch('api/Si3/Projects?username=' + this.getCookie("userSi3") + '&password=' + this.getCookie("passSi3"))
                                                        .then(response => {
                                                            if (!response.ok) {
                                                                (response.text() as Promise<String>).then(
                                                                    data => {
                                                                        alert(data);
                                                                        this.setState({ loadedDataJira: false, loading: false });
                                                                    }
                                                                )
                                                            } else {
                                                                (response.json() as Promise<Project[]>).then(
                                                                    data => {
                                                                        this.setState({ projects: data});
                                                                        fetch('api/Si3/Milestones?username=' + this.getCookie("userSi3") + "&password=" + this.getCookie("passSi3"))
                                                                            .then(response => {
                                                                                if (!response.ok) {
                                                                                    (response.text() as Promise<String>).then(
                                                                                        data => {
                                                                                            alert(data);
                                                                                            this.setState({ loadedDataJira: false, loading: false });
                                                                                        }
                                                                                    )
                                                                                } else {
                                                                                    (response.json() as Promise<Milestones[]>).then(data => {
                                                                                        this.setState({ milestones: data, loading: false, loadedDataJira: true });
                                                                                    })
                                                                                }
                                                                            });
                                                                    }                                                                
                                                                )                                                                
                                                            }
                                                        });

                                                    this.setState({
                                                        titulo: data.summary, prioridad: prioridad_,
                                                        tipo: data.issuetype, responsable: data.assignee,
                                                    });
                                                } else {
                                                    alert("Esta tarea ya está vinculada en SI3");
                                                    this.setState({ loadedDataJira: false, loading: false });
                                                }
                                            });
                                    }
                                });
                        }
                    )
                }
            });
    }

    public handleChangeProject(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ projectSelected: event.currentTarget.value, milestoneSelected: "default", todoOkProject: false });
    }
    public handleChangeMilestone(event: React.FormEvent<HTMLSelectElement>) {
        if (event.currentTarget.value == "default") {
            this.setState({ milestoneSelected: event.currentTarget.value, todoOkProject: false });
        } else this.setState({ milestoneSelected: event.currentTarget.value, todoOkProject: true })
    }
    public handleChangeProducts(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ productSelected: event.currentTarget.value, componentSelected: "default", moduleSelected: "default", todoOk: false });
    }
    public handleChangeComponents(event: React.FormEvent<HTMLSelectElement>) {

        if (event.currentTarget.value == "default") {
            this.setState({ componentSelected: event.currentTarget.value, moduleSelected: "default",todoOk: false });
        } else this.setState({ componentSelected: event.currentTarget.value, moduleSelected: "default",todoOk: true })
    }
    public handleChangeModules(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ moduleSelected: event.currentTarget.value });
    }

    public vincularProyecto() {

        this.setState({loading: true});

        var key: string;
        if (this.props.jiraKey.length > 0) {
            key = this.props.jiraKey
        } else {
            key = (this.refs["tbKeyJira"] as HTMLInputElement).value;
        }     

        fetch('api/Jira/updateIssueSi3Project?username=' + this.getCookie("userJira") + '&password=' + this.getCookie("passJira") +
            '&codeProject=' + this.state.projectSelected + '&codeMilestone=' + this.state.milestoneSelected + '&jiraKey=' + key)
            .then(response => {
                if (!response.ok) {
                    (response.text() as Promise<string>).then(data => {
                        alert(data);
                        this.setState({ loading: false });
                    })
                } else {
                    (response.text() as Promise<string>).then(data => {
                        if (this.props.jiraKey.length > 0) {
                            this.props.vincular( data, key);
                        }
                        alert("Tarea vinculada"); this.setState({ loading: false });
                    })
                    
                }
            });
    }

    public vincular() {

        this.setState({ loading: true });
        var cod = this.getCookie("codUserSi3");

        var key: string;
        if (this.props.jiraKey.length > 0) {
            key = this.props.jiraKey
        } else { key = (this.refs["tbKeyJira"] as HTMLInputElement).value; }

        fetch('api/Si3/Linkissue?username=' + this.getCookie("userSi3") + '&password=' + this.getCookie("passSi3"), {
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
            .then(response =>
            {
                if (!response.ok) {
                    (response.text() as Promise<string>).then(data => {
                        alert(data);
                        this.setState({ loading: false });
                    })
                }
                else {
                    (response.text() as Promise<string>).then(data => {
                        var issueKey = data.split("\"")[1];
                        fetch('api/Jira/updateissuesi3customfield?username=' + this.getCookie("userJira") + '&password=' + this.getCookie("passJira") + '&issueKey=' + issueKey + '&jirakey=' + key)
                            .then(response => {
                                if (!response.ok) {
                                    (response.text() as Promise<string>).then(data => {
                                        alert(data);
                                        this.setState({ loading: false });
                                    })
                                } else {
                                    if (this.props.jiraKey.length > 0) {
                                        this.props.vincular(issueKey,key);
                                    }
                                    alert("Tarea vinculada"); this.setState({ loading: false });
                                }
                            });
                    });
                }

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

    public renderInformación() {
        
        return (

            <div>
                <label><p className="ptext">Vincular a : </p></label>
                <div id="tab" className="btn-group" data-toggle="buttons-radio">
                    <a href="#tarea" className="btn btn-large btn-danger active" data-toggle="tab">Tarea</a>
                    <a href="#proyecto" className="btn btn-large btn-danger" data-toggle="tab">Proyecto</a>

                </div>

                <div className="tab-content">
                    <div className="tab-pane active" id="tarea">
                        <hr></hr>
                        <p className="ptext"><b>Título Jira :</b> {this.state.titulo} </p>
                        <p className="ptext"><b>Prioridad :</b> {this.state.prioridad}</p>
                        <p className="ptext"><b>Tipo : </b>{this.state.tipo}</p>
                        <p className="ptext"><b>Responsable : </b>{this.state.responsable}</p>
                        <label className="ptext">Producto : </label>
                        <select className="custom-select" onChange={this.handleChangeProducts} >
                            <option value="default" selected={true}>Seleccione un producto</option>
                            {
                                this.state.products.map(product =>
                                    <option key={product.code} value={product.code}>
                                        {product.name}
                                    </option>
                                )
                            }
                        </select>
                        <br></br>
                        <label className="ptext">Componente : </label>
                        <select className="custom-select" onChange={this.handleChangeComponents}>
                            <option value="default" selected={true}>Seleccione un componente</option>
                            {
                                this.state.products.filter(product => product.code == this.state.productSelected).map(p => p.componentes.map(
                                    component =>
                                        <option key={component.code} value={component.code}>
                                            {component.name}
                                        </option>
                                ))
                            }
                        </select>
                        <br></br>
                        <label className="ptext">Módulo : </label>
                        <select className="custom-select" onChange={this.handleChangeModules}>
                            <option value="default" selected={true}>Seleccione un módulo</option>
                            {
                                this.state.products.filter(product => product.code == this.state.productSelected).map(p => p.componentes.filter(
                                    component => component.code == this.state.componentSelected).map(
                                        c => c.modulos.map(modulo =>
                                            <option key={modulo.code} value={modulo.code}>
                                                {modulo.name}
                                            </option>
                                        )

                                    ))
                            }
                        </select>
                        <hr></hr>
                        <input type="button" value="Vincular" className="btn btn-primary" onClick={this.vincular} disabled={!this.state.todoOk} />
                    </div>
                    <div className="tab-pane" id="proyecto">
                        <hr></hr>
                        <label className="ptext">Proyecto : </label>
                        <select className="custom-select" onChange={this.handleChangeProject}>
                            <option value="default" selected={true}>Seleccione un proyecto</option>
                            {
                                this.state.projects.map(project =>
                                    <option key={project.code} value={project.code}>
                                        {project.title}
                                    </option>
                                )
                            }
                        </select>
                        <br></br>
                        <label className="ptext">Hito : </label>
                        <select className="custom-select" onChange={this.handleChangeMilestone}>
                            <option value="default" selected={true}>Seleccione un hito</option>
                            {
                                this.state.milestones.filter(milestone => milestone.projectCode.toString() == this.state.projectSelected).map(
                                    m => m.milestone.map(
                                        milestone =>
                                            <option key={milestone.name} value={milestone.name}>
                                                {milestone.name}
                                            </option>
                                    )
                                )
                            }
                        </select>
                        <hr></hr>
                        <input type="button" value="Vincular" className="btn btn-primary" onClick={this.vincularProyecto} disabled={!this.state.todoOkProject} />
                    </div>
                </div>                            
            </div>
            
        )
    }

    public render() {

        let formulario;
        if (this.props.jiraKey.length == 0) {
            formulario = <div>
                <form className="dataForm" onSubmit={this.handleSubmit}>
                    <label className="text">Key Jira :</label>
                    <input type="text" id="keyJira" className="form-control" name="key" ref="tbKeyJira" placeholder="Introduzca Key Jira" autoComplete="off" />
                    <input type="submit" className="btn btn-primary" value="Generar información" />
                </form>
                <br></br>
            </div>;
        }
        if (this.props.jiraKey.length > 0 && !this.state.loading && !this.state.loadedData && !this.state.loadedDataJira)
        {
            formulario = <div></div>
            this.generarInformacion();
        }

        let informacion;
        if (this.state.loadedData && this.state.loadedDataJira) {
            informacion = this.renderInformación();
        }
        const spinner = <span><ReactLoading color='#fff' type='spin' className="spinner" height={128} width={128} /></span>
        return (
            <div>
                {formulario}
                <div className="container-vincular">
                        {informacion}
                </div>

                <Loader show={this.state.loading} message={spinner} hideContentOnLoad={false} className={(this.state.loading == true) ? "overlay" : "overlay-1"} />
            </div>
        )
    }
}