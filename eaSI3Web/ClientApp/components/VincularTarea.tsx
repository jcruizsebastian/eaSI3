import * as React from "react";
import { RouteComponentProps, Route } from "react-router";
import '../css/vincularTarea.css';
import Loader from "react-loader-advanced";
import ReactLoading from "react-loading";
import { LoginGeneral } from "./LoginGeneral";
import { NavLink } from "react-router-dom";

interface Modules {
    code: string;
    name: string;
}

interface Components {
    code: string;
    name: string;
    modulos: Modules[];
}

interface Products {
    code: string;
    name: string;
    componentes: Components[];
}

interface VincularState {
    products: Products[]
    productSelected: string;
    componentSelected: string;
    moduleSelected: string;
    loadedData: boolean;
    loadedDataJira: boolean;
    titulo: string;
    prioridad: string;
    tipo: string;
    responsable: string;
    loading: boolean;
    todoOk: boolean;
}

interface Issue {
    key: string;
    summary: string;
    assignee: string;
    creator: string;
    priority: number
    issuetype: string;
    si3ID: string;
    issueId: string;
}

export class VincularTarea extends React.Component<RouteComponentProps<{}>, VincularState> {
    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.state = {
            products: [], productSelected: "", componentSelected: "", moduleSelected: "", loadedData: false,
            loadedDataJira: false, titulo: "", prioridad: "", tipo: "", loading: false, responsable: "", todoOk: false
        };

        this.handleSubmit = this.handleSubmit.bind(this);
        this.renderInformación = this.renderInformación.bind(this);
        this.handleChangeProducts = this.handleChangeProducts.bind(this);
        this.handleChangeComponents = this.handleChangeComponents.bind(this);
        this.handleChangeModules = this.handleChangeModules.bind(this);
        this.vincular = this.vincular.bind(this);
    }

    public handleSubmit(e: { preventDefault: () => void; }) {
        var keyJira: string = (this.refs["tbKeyJira"] as HTMLInputElement).value;
        
        e.preventDefault();
        this.setState({ loading: true});
        var userJira: string = "jcruiz";
        var passJira: string = "_*_d1d4ct1c97";
        var user: string = "ofjcruiz";
        var password: string = "_*_d1d4ct1c";

        fetch('api/Si3/products?username=' + user + '&password=' + password)
            .then(response => response.json() as Promise<Products[]>)
            .then(data => {
                this.setState({ products: data, loadedData: true});
                fetch('api/Jira/issue?username=' + userJira + '&password=' + passJira + '&jiraKey=' + keyJira)
                    .then(response => response.json() as Promise<Issue>)
                    .then(data => {
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
                        this.setState({ loadedDataJira: true, titulo: data.summary, prioridad: prioridad_, tipo: data.issuetype, responsable: data.assignee, loading: false });
                    });
            });

        
    }

    public handleChangeProducts(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ productSelected: event.currentTarget.value, componentSelected: "default", moduleSelected: "default", todoOk: false });
    }
    public handleChangeComponents(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ componentSelected: event.currentTarget.value, moduleSelected: "default" });
        if (event.currentTarget.value == "default") {
            this.setState({ todoOk: false });
        } else this.setState({todoOk: true})
    }
    public handleChangeModules(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ moduleSelected: event.currentTarget.value });
    }
    
    public vincular() {
        var userJira: string = "jcruiz";
        var passJira: string = "_*_d1d4ct1c97";
        var user: string = "ofjcruiz";
        var password: string = "_*_d1d4ct1c";
        this.setState({ loading: true });
        var cod = this.getCookie("codUserSi3");

        fetch('api/Si3/Linkissue?username=' + user + '&password=' + password, {
            method: 'post',
            body: JSON.stringify({
                JiraKey: (this.refs["tbKeyJira"] as HTMLInputElement).value, Titulo: this.state.titulo, Prioridad: this.state.prioridad,
                Tipo: this.state.tipo, Producto: this.state.productSelected, Componente: this.state.componentSelected, Modulo: this.state.moduleSelected,
                Responsable: this.state.responsable, CodUserSi3: cod
            }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(response => response.text() as Promise<String>)
            .then(data => {
                var issueKey = data.split("\"")[1];
                fetch('api/Jira/updateissuesi3customfield?username=' + userJira + '&password=' + passJira + '&issueKey=' + issueKey + '&jirakey=' + (this.refs["tbKeyJira"] as HTMLInputElement).value)
                    .then(data => { alert("Tarea vinculada"); this.setState({ loading: false }); })

                
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
            )
    }

    public render() {

        

        let informacion;
        if (this.state.loadedData && this.state.loadedDataJira) {
             informacion = this.renderInformación();
        }
        const spinner = <span><ReactLoading color='#fff' type='spin' className="spinner" height={128} width={128} /></span>
        return (
            <div>
                <form className="dataForm" onSubmit={this.handleSubmit}>
                    <label className="text">Key Jira :</label>
                    <input type="text" id="keyJira" className="form-control" name="key" ref="tbKeyJira" placeholder="Introduzca Key Jira" autoComplete="off" />
                    <input type="submit" className="btn btn-primary" value="Generar información"/>
                </form>
               
                {informacion}
                <Loader show={this.state.loading} message={spinner} hideContentOnLoad={false} className={(this.state.loading == true) ? "overlay" : "overlay-1"} />
            </div>
        )
    }
}