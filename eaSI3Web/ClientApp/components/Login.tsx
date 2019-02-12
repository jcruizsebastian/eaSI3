import '../css/login.css';
import * as React from 'react';
import 'isomorphic-fetch';
import 'current-week-number';
import { Checkbox } from 'react-inputs-validation';

interface UserCrendentials {
    user: string;
    password: string;
    checked: boolean;
}
interface LoginProps {
    onLogin: Function;
    isDisabled: Function;
    //calendar: Calendar;
    userProps: string;
    passwordProps: string
}


export class Login extends React.Component<LoginProps, UserCrendentials> {
   
  

    constructor(props: LoginProps) {
        super(props);

        
        this.handleSubmitLogin = this.handleSubmitLogin.bind(this);
        this.handleChangeUser = this.handleChangeUser.bind(this);
        this.handleChangePassword = this.handleChangePassword.bind(this);
        this.handleChangeCheckBox = this.handleChangeCheckBox.bind(this);

        this.state = {
            user: localStorage.getItem("userJira") as string, password: localStorage.getItem("passwordJira") as string,
            checked: false
        }
    }
   

    public handleChangeUser(event: React.FormEvent<HTMLInputElement>) {
        this.setState({ user: event.currentTarget.value });
    }

    public handleChangePassword(event: React.FormEvent<HTMLInputElement>) {
        this.setState({ password: event.currentTarget.value });
    }

    public handleSubmitLogin(e: { preventDefault: () => void; }) {
        this.props.onLogin(e, this.state.user, this.state.password, this.state.checked);
       }
   
  

    public handleChangeCheckBox(event: React.FormEvent<HTMLInputElement>) {
        this.setState({ checked: event.currentTarget.checked });
    }

  
    render()
    {
       
        return (
            <div className="form-group">
                <form className="dataForm" onSubmit={this.handleSubmitLogin}>
                    
                    <hr></hr>
                    <label htmlFor="tbUser" className="text">Nombre de usuario :</label>
                    <input type="text" id="tbUser" className="form-control" name="user" placeholder="Introduzca su nombre de usuario" onChange={this.handleChangeUser} defaultValue={this.props.userProps} />
                    <label htmlFor="tbPass" className="text">Contraseña :</label>
                    <input type="password" id="tbPass" className="form-control" name="password" placeholder="Introduzca su contraseña" onChange={this.handleChangePassword} defaultValue={this.props.passwordProps} />

                    <div>
                        <input type="checkbox" id="checkbox" onChange={this.handleChangeCheckBox} ></input>
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