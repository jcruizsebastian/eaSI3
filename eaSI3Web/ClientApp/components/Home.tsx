import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import ReactLoading from "react-loading";
import Agenda from './Agenda';
import LoginJira from './LoginJira';
import LoginSi3 from './LoginSi3';

//import { Button, FormGroup, FormControl, ControlLabel } from "react-bootstrap";

interface UserCredentials {
    user: string;
    pass: string;
    userSI3: string;
    passSI3: string;
    Weekissues: WeekJiraIssues[];
    loadedJira: boolean;
    loadingJira: boolean;
    loadedSI3: boolean;
    loadingSI3: boolean;
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
    tiempo: number;
    tiempoCorregido: number;
}

export class Home extends React.Component<RouteComponentProps<{}>, UserCredentials> {

    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.agendaModified = this.agendaModified.bind(this);
        this.onJiraDataLoaded = this.onJiraDataLoaded.bind(this);

        this.state = { user: 'jcruiz', pass: '_*_d1d4ct1c97', Weekissues: [], loadedJira: false, loadingJira: false, passSI3: '', userSI3: '', loadedSI3: false, loadingSI3: false };
    }

    
    public onJiraDataLoaded(weekJiraIssues: WeekJiraIssues[])
    {       
            this.setState({ loadedJira: true, Weekissues: weekJiraIssues });        
    }


    public agendaModified(weekJiraIssues: WeekJiraIssues[])
    {
        this.setState({ Weekissues: weekJiraIssues });
    }

    private renderAgenda(Weekissues: WeekJiraIssues[]) {

        return <Agenda weekissues={Weekissues} onAgendaModified={this.agendaModified} />        
    }

    private renderSi3(Weekissues: WeekJiraIssues[]) {
        return <LoginSi3 weekissues={Weekissues} />
    }

    public render() {

        let agenda = <p><em>Sin datos</em></p>;
        let si3;

        if (this.state.loadedJira) {

          
            agenda = this.renderAgenda(this.state.Weekissues);
            si3 = this.renderSi3(this.state.Weekissues);
        }
        
        if (!this.state.loadedJira)
            agenda = <ReactLoading color='#000' type='balls' />
        
        
        return (
            <div>

             <LoginJira onJiraLoginSuccessfully={this.onJiraDataLoaded} />
      
            {agenda} 

            {si3}

            </div>
        )
                    
    }
}
export default Home


