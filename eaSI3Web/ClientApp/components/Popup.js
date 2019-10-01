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
var Popup = /** @class */ (function (_super) {
    __extends(Popup, _super);
    function Popup(props) {
        return _super.call(this, props) || this;
    }
    Popup.prototype.render = function () {
        var _this = this;
        this.props.data.map(function (ax) { return console.log(ax) + "-"; });
        return (React.createElement("div", { className: "popup_alert" },
            React.createElement("div", { className: "popup_inner_alert" },
                React.createElement("div", { className: "popup_information" }, this.props.error ?
                    React.createElement("div", null,
                        React.createElement("div", null,
                            React.createElement("img", { src: "error.png", width: "200" }),
                            React.createElement("div", { className: "popup_information_text" }, this.props.data.map(function (ax) { return React.createElement("div", null, ax.toString()); }))),
                        React.createElement("button", { onClick: function () { _this.props.closePopup(); }, className: "btn-popup-close-error" }, "Cerrar"))
                    :
                        React.createElement("div", null,
                            React.createElement("div", null,
                                React.createElement("img", { src: "not_error.png", width: "200", className: "img_not_error" }),
                                React.createElement("div", { className: "popup_information_text" }, this.props.data)),
                            React.createElement("button", { onClick: function () { _this.props.closePopup(); }, className: "btn-popup-close" }, "Cerrar"))))));
    };
    return Popup;
}(React.Component));
export { Popup };
//# sourceMappingURL=Popup.js.map