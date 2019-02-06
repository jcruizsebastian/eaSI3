import '../css/login.css';
import * as React from 'react';
import 'isomorphic-fetch';
import 'current-week-number';

interface UserCrendentials {
    user: string;
    password: string;
}
interface LoginProps {
    onLogin: Function;
    isDisabled: Function; 
   
  
}

export class Login extends React.Component<LoginProps, UserCrendentials> {

    constructor(props: LoginProps) {
        super(props);
        
        this.handleSubmitLogin = this.handleSubmitLogin.bind(this);
        this.handleChangeUser = this.handleChangeUser.bind(this);
        this.handleChangePassword = this.handleChangePassword.bind(this);
    }

    public handleChangeUser(event: React.FormEvent<HTMLInputElement>) {
        this.setState({ user: event.currentTarget.value });
    }

    public handleChangePassword(event: React.FormEvent<HTMLInputElement>) {
        this.setState({ password: event.currentTarget.value });
    }

    public handleSubmitLogin(e: { preventDefault: () => void; }) {

        this.props.onLogin(e,this.state.user, this.state.password);
    }


    render()
    {
        
        return (
            <div className="form-group">
                <form className="dataForm" onSubmit={this.handleSubmitLogin}>
                    
                    <hr></hr>
                    <label htmlFor="tbUser">Nombre de usuario</label>
                    <input type="text" id="tbUser" className="form-control" name="user" placeholder="Introduzca su nombre de usuario"  onChange={this.handleChangeUser} />
                    <label htmlFor="tbPass">Contraseña</label>
                    <input type="password" id="tbPass" className="form-control" name="password" placeholder="Introduzca su contraseña" onChange={this.handleChangePassword} />

                    <hr></hr>
                    <input disabled={this.props.isDisabled()} type="submit" className="btn btn-primary" value="Enviar" />
                </form>
            </div>
        )
    }
}
export default Login