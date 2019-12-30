import 'bootstrap';
import * as React from 'react';
import Loader from 'react-loader-advanced';
import ReactLoading from "react-loading";
import '../css/logingeneral.css';
import { LoginProps } from './Model/Props/LoginProps';
import { LoginState } from './Model/States/LoginState';
import { User } from './Model/User';
import { Cube } from './Cube';

export class LoginGeneral extends React.Component<LoginProps, LoginState> {
    constructor(props: any) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.metodo = this.metodo.bind(this);
        this.ValidateLogin = this.ValidateLogin.bind(this);
        this.state = { users: [], userJiraLoaded: false, userSi3Loaded: false, loading: false };
    }


    componentDidMount() {
        this.metodo();
    }

    public metodo() {
        fetch('api/Si3/users')
            .then(response => response.json() as Promise<User[]>)
            .then(data => {
                this.setState({ users: data });
            });
    }

    public handleSubmit(e: { preventDefault: () => void; }) {
        e.preventDefault();

        var userJira = (this.refs["tbUserJira"] as HTMLInputElement).value;
        var passJira = (this.refs["tbPassJira"] as HTMLInputElement).value;
        var userSi3 = (this.refs["tbUserSi3"] as HTMLInputElement).value;
        var passSi3 = (this.refs["tbPassSi3"] as HTMLInputElement).value;

        this.setState({ loading: true });

        let urlJira = 'api/Jira/Login?';
        let urlSi3 = 'api/Si3/Login?'


        fetch(urlJira, {
            method: 'post',
            body: JSON.stringify({ usernameJira: userJira, passwordJira: passJira, usernameSi3: userSi3, passwordSi3: passSi3}),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(response => {
                if (!response.ok) {
                    (response.text() as Promise<String>).then(
                        data => { alert(data); this.setState({ userJiraLoaded: false, loading: false }); }
                    );
                }
                else { this.setState({ userJiraLoaded: true }); }
            })
            .then(data => {
                fetch(urlSi3, {
                    method: 'post',
                    body: JSON.stringify({ usernameJira: userJira, passwordJira: passJira,usernameSi3: userSi3, passwordSi3: passSi3}),
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                })
                    .then(response => {
                        if (!response.ok) {
                            (response.text() as Promise<String>).then(
                                data => { alert(data); this.setState({ userSi3Loaded: false, loading: false }); }
                            );
                        }
                        else {
                            (response.json() as Promise<Number>).then(
                                data => {
                                    this.setState({ userSi3Loaded: true });
                                    this.ValidateLogin(data);
                                })
                            
                        }
                        
                    });
            });
    }

    public ValidateLogin(idUser: Number) {

        var codUserSi3 = (this.refs["tbCodUserSi3"] as HTMLSelectElement).value;
        var name = (((this.refs["tbCodUserSi3"] as HTMLSelectElement).children.item((this.refs["tbCodUserSi3"] as HTMLSelectElement).selectedIndex)) as HTMLOptionElement).innerText;
        
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

        } else { this.setState({ loading: false });}
    }

    render() {
        const spinner = <span><ReactLoading color='#fff' type='spin' className="spinner" height={128} width={128} /></span>
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
                    <div><select ref="tbCodUserSi3">
                        <option value="default">Seleccione un usuario</option>
                        {
                            this.state.users.map(user =>
                                <option key={user.codigo} value={user.codigo}>
                                    {user.nombre}
                                </option>
                            )
                        }
                    </select></div>
                    <div><input type="text" name="name" ref="tbUserSi3" placeholder="Introduzca usuario de Si3" autoComplete="off" /></div>
                    <div><input type="password" name="name" ref="tbPassSi3" placeholder="Introduzca contraseña de Si3" autoComplete="off" /></div>
                        <div><input type="submit" name="submit" value="Iniciar Sesión" className="btn btn-primary" /></div>
                    </div>
                </form>
                <Loader show={this.state.loading} message={<Cube />} hideContentOnLoad={false} className={(this.state.loading == true) ? "overlay" : "overlay-1"} />
            </div>
        )
    }
}