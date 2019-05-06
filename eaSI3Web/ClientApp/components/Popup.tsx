import * as React from "react";
import { PopupAlertProps } from "./Model/Props/PopupAlertProps";

export class Popup extends React.Component<PopupAlertProps, {}>{

    constructor(props: PopupAlertProps) {
        super(props);
    }
    
    public render() {
        //this.props.data.map(ax => console.log(ax) + "-");


        return (
            <div className="popup_alert">
                <div className="popup_inner_alert">
                    <div className="popup_information">
                        {this.props.error ?
                            <div>
                                <img src="error.png" width="200" />
                                <div className="popup_information_text">
                                    {this.props.data[0]}
                                </div>
                            </div>
                            :
                            <div>
                                <img src="not_error.png" width="200" className="img_not_error" />
                                <div className="popup_information_text">
                                    {this.props.data}
                                </div>
                            </div>
                        }
                        <button onClick={() => { this.props.closePopup() }} className="btn-popup-close">Cerrar</button>
                    </div>
                    
                </div>
            </div>)
    }
}