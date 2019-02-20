import * as React from 'react';
import { NavMenu } from './NavMenu';
import { LoginGeneral } from './LoginGeneral';
import { Route, Redirect } from 'react-router'
export interface LayoutProps {
    children?: React.ReactNode;
}

interface LayoutState {
    logged: boolean
}
export class Layout extends React.Component<LayoutProps, LayoutState> {
    constructor(props: LayoutProps) {
        super(props);
        this.onLogin = this.onLogin.bind(this);
        this.state = { logged: false };
    }

    public onLogin() { this.setState({ logged: true }); }
    public render() {
        var style = { backgroundColor: '#222', height: '50px'};
        let home;

        if (document.cookie.length == 0)
        {
            home = <LoginGeneral onLogin={this.onLogin} />
        }
        else {
            home = <div>
                <div className='row'>
                    <div className='col-sm-12' style={style}>
                    </div>
                </div>

                <div className='row'>
                    <div className='col-sm-3'>
                        <NavMenu />
                    </div>
                     <div className='col-sm-9'>
                        {this.props.children}
                    </div>
                </div>

            </div>
        }

        return <div className='container-fluid' >
            {home}
            </div>
            
    }
}
