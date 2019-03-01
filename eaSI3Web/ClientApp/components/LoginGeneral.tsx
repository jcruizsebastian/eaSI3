import 'bootstrap';
import * as React from 'react';
import Loader from 'react-loader-advanced';
import ReactLoading from "react-loading";
import '../css/logingeneral.css';
import { LoginProps } from './Model/Props/LoginProps';
import { LoginState } from './Model/States/LoginState';
import { User } from './Model/User';

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

        fetch('api/Jira/validateLogin?username=' + userJira + '&password=' + passJira)
            .then(response => {
                if (!response.ok) {
                    (response.text() as Promise<String>).then(
                        data => { alert(data); this.setState({ userJiraLoaded: false }); }
                    );
                }
                else { this.setState({ userJiraLoaded: true }); }
            })
            .then(data => {
                fetch('api/Si3/validateLogin?username=' + userSi3 + '&password=' + passSi3)
                    .then(response => {
                        if (!response.ok) {
                            (response.text() as Promise<String>).then(
                                data => { alert(data); this.setState({ userSi3Loaded: false }); }
                            );
                        }
                        else { this.setState({ userSi3Loaded: true }); }
                        this.ValidateLogin();
                    });
            });
    }

    public ValidateLogin() {
        var userJira = (this.refs["tbUserJira"] as HTMLInputElement).value;
        var passJira = (this.refs["tbPassJira"] as HTMLInputElement).value;
        var codUserSi3 = (this.refs["tbCodUserSi3"] as HTMLSelectElement).value;
        var userSi3 = (this.refs["tbUserSi3"] as HTMLInputElement).value;
        var passSi3 = (this.refs["tbPassSi3"] as HTMLInputElement).value;
        var name = (((this.refs["tbCodUserSi3"] as HTMLSelectElement).children.item((this.refs["tbCodUserSi3"] as HTMLSelectElement).selectedIndex)) as HTMLOptionElement).innerText;

        if (this.state.userJiraLoaded && this.state.userSi3Loaded && codUserSi3 != "default") {

            this.setState({ loading: false });
            var expiration_date = new Date();
            expiration_date.setFullYear(expiration_date.getFullYear() + 1);

            document.cookie = "userJira=" + userJira + "; expires=" + expiration_date;
            document.cookie = "passJira=" + passJira + "; expires=" + expiration_date;
            document.cookie = "codUserSi3=" + codUserSi3 + "; expires=" + expiration_date;
            document.cookie = "userSi3=" + userSi3 + "; expires=" + expiration_date;
            document.cookie = "passSi3=" + passSi3 + "; expires=" + expiration_date;

            this.props.onLogin(name);

        } else { this.setState({ loading: false });}
    }

    render() {
        const spinner = <span><ReactLoading color='#fff' type='spin' className="spinner" height={128} width={128} /></span>
        return (
            <div className="bodyLogin">
                <form className="formulario" onSubmit={this.handleSubmit}>
                    <img src="logo_open.png" width="156" height="94" className="img" />
                    <div><label>Usuario de Jira : <input type="text" name="name" ref="tbUserJira" placeholder="Introduzca usuario de Jira" autoComplete="off" /></label></div>
                    <div><label>Contraseña de Jira : <input type="password" name="name" ref="tbPassJira" placeholder="Introduzca contraseña de Jira" autoComplete="off" /></label></div>
                    <div><label>Nombre de usuario : <select ref="tbCodUserSi3">
                        <option value="default">Seleccione un usuario</option>
                        {
                            this.state.users.map(user =>
                                <option key={user.codigo} value={user.codigo}>
                                    {user.nombre}
                                </option>
                            )
                        }
                    </select></label></div>
                    <div><label>Usuario de Si3 : <input type="text" name="name" ref="tbUserSi3" placeholder="Introduzca usuario de Si3" autoComplete="off" /></label></div>
                    <div><label>Contraseña de Si3 : <input type="password" name="name" ref="tbPassSi3" placeholder="Introduzca contraseña de Si3" autoComplete="off" /></label></div>
                    <div><input type="submit" name="submit" value="Iniciar Sesión" className="btn btn-primary" /></div>
                </form>
                <Loader show={this.state.loading} message={spinner} hideContentOnLoad={false} className={(this.state.loading == true) ? "overlay" : "overlay-1"} />
            </div>
        )
    }
}