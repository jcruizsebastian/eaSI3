import '../css/login.css';
import * as React from 'react';
import 'isomorphic-fetch';
import 'current-week-number';

interface UserCrendentials {
    user: string;
    password: string;
    selectedWeek: string;
}
interface LoginProps {
    onLogin: Function;
    isDisabled: Function;
    calendar: Calendar;
}

interface CalendarWeeks {
    numberWeek: number;
    description: string;
    starOfWeek: Date;
    endOfWeek: Date;
}

interface Calendar {
    weeks: CalendarWeeks[];
}

export class Login extends React.Component<LoginProps, UserCrendentials> {
   
  

    constructor(props: LoginProps) {
        super(props);

        
        this.handleSubmitLogin = this.handleSubmitLogin.bind(this);
        this.handleChangeUser = this.handleChangeUser.bind(this);
        this.handleChangePassword = this.handleChangePassword.bind(this);
        this.handleChangeWeek = this.handleChangeWeek.bind(this);

        this.state = { user: "", password: "", selectedWeek: this.props.calendar.weeks.length.toString() }
    }
   

    public handleChangeUser(event: React.FormEvent<HTMLInputElement>) {
        this.setState({ user: event.currentTarget.value });
    }

    public handleChangePassword(event: React.FormEvent<HTMLInputElement>) {
        this.setState({ password: event.currentTarget.value });
    }

    public handleSubmitLogin(e: { preventDefault: () => void; }) {

        this.props.onLogin(e, this.state.user, this.state.password, this.state.selectedWeek);
    }

    public handleChangeWeek(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ selectedWeek: event.currentTarget.value });
        
    }

    
    
    render()
    {
        return (
            <div className="form-group">
                <form className="dataForm" onSubmit={this.handleSubmitLogin}>
                    
                    <hr></hr>
                    <label htmlFor="tbUser" className="text">Nombre de usuario :</label>
                    <input type="text" id="tbUser" className="form-control" name="user" placeholder="Introduzca su nombre de usuario"  onChange={this.handleChangeUser} />
                    <label htmlFor="tbPass" className="text">Contraseña :</label>
                    <input type="password" id="tbPass" className="form-control" name="password" placeholder="Introduzca su contraseña" onChange={this.handleChangePassword} />
                    <label>Elija semana de trabajo :</label>
                    <select className="calendar" onChange={this.handleChangeWeek}>
                        {
                            this.props.calendar.weeks.map(week =>
                                <option value={week.numberWeek} key={week.numberWeek} selected={(week.numberWeek == this.props.calendar.weeks.length) ? true : false} >
                                    {week.description}
                                </option>)
                        }
                    </select>
                    <hr></hr>
                    <input disabled={this.props.isDisabled()} type="submit" className="btn btn-primary" value="Enviar" />
                </form>
            </div>
        )
    }
}
export default Login