export = index;
declare class index {
    static displayName: string;
    constructor(e: any);
    checkAllColors(e: any): any;
    checkColors(e: any, t: any): void;
    forceUpdate(callback: any): void;
    handleClick(e: any): void;
    interpolateColorWithHover(e: any, t: any, n: any): any;
    makeStyle(e: any, t: any): any;
    onMouseOut(): void;
    onMouseOver(): void;
    render(): any;
    setState(partialState: any, callback: any): void;
}
declare namespace index {
    namespace defaultProps {
        const activeLabel: string;
        const activeLabelStyle: {};
        const activeLabelStyleHover: {};
        function animateThumbStyleHover(): any;
        function animateThumbStyleToggle(): any;
        function animateTrackStyleHover(): any;
        function animateTrackStyleToggle(): any;
        const colors: {
            active: {
                base: string;
                hover: string;
            };
            activeThumb: {
                base: string;
                hover: string;
            };
            inactive: {
                base: string;
                hover: string;
            };
            inactiveThumb: {
                base: string;
                hover: string;
            };
        };
        const containerStyle: {};
        const inactiveLabel: string;
        const inactiveLabelStyle: {};
        const inactiveLabelStyleHover: {};
        const internalHoverSpringSetting: {
            damping: number;
            stiffness: number;
        };
        const internalSpringSetting: {
            damping: number;
            stiffness: number;
        };
        function onToggle(): void;
        const passThroughInputProps: {};
        const thumbAnimateRange: number[];
        const thumbStyle: {};
        const thumbStyleHover: {};
        const trackStyle: {};
        const trackStyleHover: {};
        const value: boolean;
    }
    namespace propTypes {
        function activeLabel(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace activeLabel {
            // Circular reference from index.propTypes.activeLabel
            const isRequired: any;
        }
        function activeLabelStyle(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace activeLabelStyle {
            // Circular reference from index.propTypes.activeLabelStyle
            const isRequired: any;
        }
        function activeLabelStyleHover(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace activeLabelStyleHover {
            // Circular reference from index.propTypes.activeLabelStyleHover
            const isRequired: any;
        }
        function activeThumbStyle(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace activeThumbStyle {
            // Circular reference from index.propTypes.activeThumbStyle
            const isRequired: any;
        }
        function activeThumbStyleHover(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace activeThumbStyleHover {
            // Circular reference from index.propTypes.activeThumbStyleHover
            const isRequired: any;
        }
        function animateThumbStyleHover(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace animateThumbStyleHover {
            // Circular reference from index.propTypes.animateThumbStyleHover
            const isRequired: any;
        }
        function animateThumbStyleToggle(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace animateThumbStyleToggle {
            // Circular reference from index.propTypes.animateThumbStyleToggle
            const isRequired: any;
        }
        function animateTrackStyleHover(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace animateTrackStyleHover {
            // Circular reference from index.propTypes.animateTrackStyleHover
            const isRequired: any;
        }
        function animateTrackStyleToggle(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace animateTrackStyleToggle {
            // Circular reference from index.propTypes.animateTrackStyleToggle
            const isRequired: any;
        }
        function colors(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace colors {
            // Circular reference from index.propTypes.colors
            const isRequired: any;
        }
        function containerStyle(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace containerStyle {
            // Circular reference from index.propTypes.containerStyle
            const isRequired: any;
        }
        function inactiveLabel(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace inactiveLabel {
            // Circular reference from index.propTypes.inactiveLabel
            const isRequired: any;
        }
        function inactiveLabelStyle(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace inactiveLabelStyle {
            // Circular reference from index.propTypes.inactiveLabelStyle
            const isRequired: any;
        }
        function inactiveLabelStyleHover(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace inactiveLabelStyleHover {
            // Circular reference from index.propTypes.inactiveLabelStyleHover
            const isRequired: any;
        }
        function internalHoverSpringSetting(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace internalHoverSpringSetting {
            // Circular reference from index.propTypes.internalHoverSpringSetting
            const isRequired: any;
        }
        function internalSpringSetting(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace internalSpringSetting {
            // Circular reference from index.propTypes.internalSpringSetting
            const isRequired: any;
        }
        function onClick(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace onClick {
            // Circular reference from index.propTypes.onClick
            const isRequired: any;
        }
        function onToggle(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace onToggle {
            // Circular reference from index.propTypes.onToggle
            const isRequired: any;
        }
        function passThroughInputProps(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace passThroughInputProps {
            // Circular reference from index.propTypes.passThroughInputProps
            const isRequired: any;
        }
        function thumbAnimateRange(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace thumbAnimateRange {
            // Circular reference from index.propTypes.thumbAnimateRange
            const isRequired: any;
        }
        function thumbIcon(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace thumbIcon {
            // Circular reference from index.propTypes.thumbIcon
            const isRequired: any;
        }
        function thumbStyle(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace thumbStyle {
            // Circular reference from index.propTypes.thumbStyle
            const isRequired: any;
        }
        function thumbStyleHover(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace thumbStyleHover {
            // Circular reference from index.propTypes.thumbStyleHover
            const isRequired: any;
        }
        function trackStyle(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace trackStyle {
            // Circular reference from index.propTypes.trackStyle
            const isRequired: any;
        }
        function trackStyleHover(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace trackStyleHover {
            // Circular reference from index.propTypes.trackStyleHover
            const isRequired: any;
        }
        function value(e: any, t: any, n: any, r: any, i: any, l: any): void;
        namespace value {
            // Circular reference from index.propTypes.value
            const isRequired: any;
        }
    }
}