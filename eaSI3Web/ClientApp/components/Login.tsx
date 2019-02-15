﻿import '../css/login.css';
import * as React from 'react';
import 'isomorphic-fetch';
import 'current-week-number';
import { Checkbox } from 'react-inputs-validation';


interface LoginProps {
    onLogin: Function;
    isDisabled: Function;
    //calendar: Calendar;
    userProps: string;
    passwordProps: string
}

export class Login extends React.Component<LoginProps, {}> {

    constructor(props: LoginProps) {
        console.log("Entra en constructor de Login");
        super(props);
        
        this.handleSubmitLogin = this.handleSubmitLogin.bind(this);

        this.state = {
            user: localStorage.getItem("userJira") as string, password: localStorage.getItem("passwordJira") as string,
            checked: false
        }
    }


    public handleSubmitLogin(e: { preventDefault: () => void; }) {
        console.log("Entra en handleSubmitLogin de Login");
        this.props.onLogin(e, (this.refs["tbUser"] as HTMLInputElement).value, (this.refs["tbPass"] as HTMLInputElement).value, (this.refs["tbCheck"] as HTMLInputElement).checked);
    }

    render() {
        console.log("Entra en render de Login");

        return (
            <div className="form-group">
                <form className="dataForm" onSubmit={this.handleSubmitLogin}>

                    <hr></hr>
                    <label htmlFor="tbUser" className="text">Nombre de usuario :</label>
                    <input type="text" id="tbUser" className="form-control" name="user" ref="tbUser" placeholder="Introduzca su nombre de usuario" defaultValue={this.props.userProps} autoComplete = "off"/>
                    <label htmlFor="tbPass" className="text">Contraseña :</label>
                    <input type="password" id="tbPass" className="form-control" name="password" ref="tbPass" placeholder="Introduzca su contraseña" defaultValue={this.props.passwordProps} autoComplete="off" />

                    <div>
                        <input type="checkbox" id="checkbox" ref="tbCheck"></input>
                        <label>Recordar usuario y contraseña</label>
                    </div>
                    <hr></hr>
                    <input disabled={this.props.isDisabled()} type="submit" className="btn btn-primary" value="Enviar" />
                </form>
            </div>
        )
    }
}
export default Login