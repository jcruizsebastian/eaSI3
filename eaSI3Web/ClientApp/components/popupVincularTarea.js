var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
import * as React from "react";
import { VincularTarea } from "./VincularTarea";
import * as ReactDOM from "react-dom";
var PopupVincularTarea = /** @class */ (function (_super) {
    __extends(PopupVincularTarea, _super);
    function PopupVincularTarea(props) {
        var _this = _super.call(this, props) || this;
        _this.state = { idSi3: "", key: "" };
        _this.vincular = _this.vincular.bind(_this);
        return _this;
    }
    PopupVincularTarea.prototype.componentDidMount = function () {
        var informacion = ReactDOM.findDOMNode(this).querySelector(".container-vincular-informacion");
        informacion.style.marginLeft = "0px";
    };
    PopupVincularTarea.prototype.vincular = function (idSi3, key) {
        this.setState({ idSi3: idSi3, key: key });
    };
    PopupVincularTarea.prototype.render = function () {
        var _this = this;
        return (React.createElement("div", { className: "popup" },
            React.createElement("div", { className: "popup_inner" },
                React.createElement("button", { type: "button", id: "close", className: "btn btn-danger btn-sm", onClick: function () { _this.props.closePopup(_this.state.idSi3, _this.state.key); } }, "X"),
                React.createElement(VincularTarea, { jiraKey: this.props.keyJira, vincular: this.vincular }))));
    };
    return PopupVincularTarea;
}(React.Component));
export { PopupVincularTarea };
//# sourceMappingURL=popupVincularTarea.js.map