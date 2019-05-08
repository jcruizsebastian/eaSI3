import * as React from "react";
import { popupProps } from './Model/Props/popupProps';
import { VincularTarea } from "./VincularTarea";
import { PopupState } from './Model/States/PopupState';
import { RouteProps, RouteComponentProps } from 'react-router-dom';
import * as ReactDOM from "react-dom";
export class PopupVincularTarea extends React.Component<popupProps, PopupState>{

    constructor(props: popupProps) {
        super(props);
        this.state = { idSi3: "", key:"" };
        this.vincular = this.vincular.bind(this);
    }
    componentDidMount() {
        var informacion = (ReactDOM.findDOMNode(this) as Element).querySelector(".container-vincular-informacion") as HTMLDivElement;
        informacion.style.marginLeft = "0px";
    }
    public vincular(idSi3: string, key: string) {
        this.setState({ idSi3: idSi3, key:key });
    }

    public render() {
        return (
            <div className="popup">
                <div className="popup_inner">
                    <button type="button" id="close" className="btn btn-danger btn-sm" onClick={() => { this.props.closePopup(this.state.idSi3, this.state.key) }}>X</button>
                    <VincularTarea jiraKey={this.props.keyJira} vincular={this.vincular} />
                </div>
            </div>
        )
    }
}