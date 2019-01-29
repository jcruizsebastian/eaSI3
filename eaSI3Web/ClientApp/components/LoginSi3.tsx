import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";

interface UserCredentials {
    
    user: string;
    pass: string;
    userSI3: string;
    passSI3: string;
    Weekissues: WeekJiraIssues[];

}

interface WeekJiraIssues {
    fecha: Date;
    issues: JiraIssues[];
}

interface JiraIssues {
    issueSi3: string;
    issueCode: string;
    issueKey: string;
    titulo: string;
    tiempo: string;
    tiempoCorregido: string;
}

interface WeekJiraIssuesProps {
    weekissues: WeekJiraIssues[];
}

export class LoginSi3 extends React.Component<WeekJiraIssuesProps, UserCredentials> {

    constructor(props: WeekJiraIssuesProps) {
        super(props);

        this.handleSubmitSi3 = this.handleSubmitSi3.bind(this);
        this.handleChangeUserSI3 = this.handleChangeUserSI3.bind(this);
        this.handleChangePassSI3 = this.handleChangePassSI3.bind(this);

        this.state = { user: 'jcruiz', pass: '_*_d1d4ct1c97', Weekissues: this.props.weekissues, passSI3: '', userSI3: ''};
    }

    public handleChangeUserSI3(event: React.FormEvent<HTMLInputElement>) {

        this.setState({ userSI3: event.currentTarget.value });
    }

    public handleChangePassSI3(event: React.FormEvent<HTMLInputElement>) {

        this.setState({ passSI3: event.currentTarget.value });
    }

    public handleSubmitSi3(e: { preventDefault: () => void; }) {

        e.preventDefault();

        fetch('api/SI3/register?username=' + this.state.userSI3 + '&password=' + this.state.passSI3, {
            method: 'post',
            body: JSON.stringify(this.state.Weekissues),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(res => {
                console.log(res);
            });
    }
    render() {

        return (
            <div>
            <form className="si3Form" onSubmit={this.handleSubmitSi3}>
                <input type="text" id="tbUserSI3" name="tbUserSI3" value={this.state.userSI3} onChange={this.handleChangeUserSI3} placeholder='si3 user' />
                <input type="password" id="tbPassSI3" name="tbPassSI3" value={this.state.passSI3} onChange={this.handleChangePassSI3} placeholder='si3 pass' />
                <input type="submit" value="Enviar a SI3" />
                </form>
            </div>
            )
    }

}

export default LoginSi3