import * as React from 'react';
import ReactLoading from "react-loading";
import { LoginGeneral } from './LoginGeneral';
import { LayoutState } from './Model/States/LayoutState';
import { NavMenu } from './NavMenu';
import { Link } from 'react-router-dom';
import { LoginJira } from './LoginJira';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class Layout extends React.Component<LayoutProps, LayoutState> {

    constructor(props: LayoutProps) {
        super(props);
        this.onLogin = this.onLogin.bind(this);
        this.getCookie = this.getCookie.bind(this);
        this.logout = this.logout.bind(this);
        this.validate = this.validate.bind(this);
        this.setCookie = this.setCookie.bind(this);

        this.state = { logged: false, cookiesOk: false, name: "", loaded: false, registered: true };
    }
    async componentDidMount() {
        await this.validate();
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

    public validate() {
        if (this.getCookie("codUserSi3") == "" || this.getCookie("userId") == "") {
            this.setCookie("_ga", "", 0);
            this.setCookie("userJira", "", 0);
            this.setCookie("passJira", "", 0);
            this.setCookie("userSi3", "", 0);
            this.setCookie("passSi3", "", 0);
            this.setCookie("codUserSi3", "", 0);
            this.setCookie("userId", "", 0);
        }

        this.setState({ loaded: false });

        fetch('api/Jira/ValidateLogin', {
            method: 'post',
            body: JSON.stringify({ username: "", password: "" }),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(response => {
                if (!response.ok) {
                    (response.text() as Promise<String>).then(data =>
                    {
                        fetch('/api/Jira/ExistsUser').then(response => {
                            if (!response.ok) {
                                alert(data);
                                this.setState({ loaded: true, cookiesOk: false });
                                this.logout();
                            } else {
                                this.setState({ loaded: true, cookiesOk: false, registered: true });
                            }
                        });

                    });
                } else {

                    fetch('api/Si3/ValidateLogin?', {
                        method: 'post',
                        body: JSON.stringify({ username: "", password: "" }),
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        },
                    })
                        .then(response => {
                            if (!response.ok) {
                                this.setState({ cookiesOk: false, loaded: true });
                                this.logout();
                            } else {
                                this.setState({ cookiesOk: true, loaded: true });
                            }
                        });                       
                }             
            });             
    }

    //función para cambiar una cookie
    public setCookie(cname: string, cvalue: string, exdays: number) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    }

    public onLogin(nameUser: string) {
        localStorage.removeItem('name');
        localStorage.setItem("name", nameUser);
        this.setState({ logged: true, cookiesOk: true });
    }

    public logout() {

        this.setCookie("userId", "", 0);
        this.setCookie("codUserSi3", "", 0);

        this.setState({ logged: false });
    }
    public render() {

        let home;

        if (this.state.loaded) {
            if (this.getCookie("codUserSi3") == "" || this.getCookie("userId") == "") {
                home = <LoginGeneral onLogin={this.onLogin} />
            }
            else if (this.state.cookiesOk) {
                var name = localStorage.getItem('name');

                home = <div>
                    <div className='row'>
                        <div className='col-sm-12'>
                            <Link to={'/'} style={{ color: "white", marginLeft: "10px", marginBottom: "-20px" }}>
                                <span className='glyphicon glyphicon-chevron-left' ></span> Volver
                            </Link>
                            <input type="button" className="btn-logout" id="logout" value="Log out" onClick={this.logout} />
                            <label id="name">{name}</label>
                        </div>
                    </div>

                    <div className='row'>
                        <div className='container-fluid-navmenu'>
                            {this.props.children}
                        </div>
                    </div>

                </div>
            } else if (this.state.registered) {
                home = <LoginJira onLogin={this.onLogin} />
            } else { home = <LoginGeneral onLogin={this.onLogin} /> }
        }
        
        return <div className='container-fluid' >
            {home}
        </div>

    }
}
