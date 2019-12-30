import * as React from 'react';
import Loader from 'react-loader-advanced';
import { Cube } from './Cube';
import { LoginJiraState } from './Model/States/LoginJiraState';
import { LoginProps } from './Model/Props/LoginProps';

export class LoginJira extends React.Component<LoginProps, LoginJiraState> {

    constructor(props: any) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.Login = this.Login.bind(this);

        this.state = { loading: false, userJiraLoaded: false };
    }

    public handleSubmit(e: { preventDefault: () => void; }) {
        e.preventDefault();
        this.setState({ loading: true });        
        var userJira = (this.refs["tbUserJira"] as HTMLInputElement).value;
        var passJira = (this.refs["tbPassJira"] as HTMLInputElement).value;
        var userSi3 = "";
        var passSi3 = "";
        let urlJira = 'api/Jira/Login?';

        fetch(urlJira, {
            method: 'post',
            body: JSON.stringify({ usernameJira: userJira, passwordJira: passJira, usernameSi3: userSi3, passwordSi3: passSi3 }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(response => {
                if (!response.ok) {
                    (response.text() as Promise<String>).then(
                        data => { alert(data);this.setState({ userJiraLoaded: false, loading: false }); }
                    );
                }
                else { this.Login(); }
            })
    }

    public Login() {
        var name = "";
        fetch("/api/Jira/GetUserName").then(response => {
            if (response.ok) {
                (response.text() as Promise<string>).then(
                    data => {
                        name = data;
                        fetch("/api/Jira/GetCodUser").then(response => {
                            if (response.ok) {
                                (response.json() as Promise<Number>).then(data => {                                    
                                    var expiration_date = new Date();
                                    expiration_date.setFullYear(expiration_date.getFullYear() + 1);
                                    document.cookie = "codUserSi3=" + data + "; path=/;" + "expires=" + expiration_date;
                                    this.props.onLogin(name);
                                });
                            }
                        })
                    }
                );
            }
        });
    }

    public render() {
        return (
            <div className="bodyLogin">
                <form className="formulario" onSubmit={this.handleSubmit}>
                    <div className="form1">
                        <img src="logo_open.png" width="200" height="75" className="img" />
                        <div><label>Si accede a eaSi3 podrá imputar sus horas de trabajo de una forma rápida y sencilla.</label></div>
                    </div>
                    <div className="form2">
                        <div><input type="text" name="name" ref="tbUserJira" placeholder="Introduzca usuario de Jira" autoComplete="off" /></div>
                        <div><input type="password" name="name" ref="tbPassJira" placeholder="Introduzca contraseña de Jira" autoComplete="off" /></div>
                        <div><input type="submit" name="submit" value="Iniciar Sesión" className="btn btn-primary" /></div>
                    </div>
                </form>
                <Loader show={this.state.loading} message={<Cube />} hideContentOnLoad={false} className={(this.state.loading == true) ? "overlay" : "overlay-1"} />
            </div>)
    }
}