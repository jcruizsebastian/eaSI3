export class Checkbox {
    static getDerivedStateFromProps(nextProps: any, prevState: any): any;
    constructor(props: any);
    check(): void;
    componentDidUpdate(prevProps: any, prevState: any): void;
    forceUpdate(callback: any): void;
    handleCheckEnd(err: any, message: any): void;
    onBlur(e: any): void;
    onChange(e: any): void;
    onClick(e: any): void;
    onFocus(e: any): void;
    render(): any;
    setState(partialState: any, callback: any): void;
}
export namespace Checkbox {
    namespace defaultProps {
        const checked: boolean;
        const classNameContainer: string;
        const classNameInput: string;
        const classNameInputBox: string;
        const classNameWrapper: string;
        const customStyleContainer: {};
        const customStyleInput: {};
        const customStyleInputBox: {};
        const customStyleWrapper: {};
        const disabled: boolean;
        const id: string;
        const labelHtml: any;
        const name: string;
        function onChange(): void;
        const tabIndex: number;
        const validate: boolean;
        const validationOption: {};
        const value: string;
    }
}
export class Radiobox {
    static getDerivedStateFromProps(nextProps: any, prevState: any): any;
    constructor(props: any);
    check(...args: any[]): void;
    componentDidUpdate(prevProps: any, prevState: any): void;
    forceUpdate(callback: any): void;
    handleCheckEnd(err: any, message: any): void;
    onBlur(e: any): void;
    onChange(val: any, e: any): void;
    onClick(e: any): void;
    onFocus(e: any): void;
    render(): any;
    setState(partialState: any, callback: any): void;
}
export namespace Radiobox {
    namespace defaultProps {
        const classNameContainer: string;
        const classNameInput: string;
        const classNameOptionListItem: string;
        const classNameWrapper: string;
        const customStyleContainer: {};
        const customStyleInput: {};
        const customStyleOptionListItem: {};
        const customStyleWrapper: {};
        const disabled: boolean;
        const id: string;
        const name: string;
        function onChange(): void;
        const optionList: any[];
        const tabIndex: number;
        const validate: boolean;
        const validationOption: {};
        const value: string;
    }
}
export class Select {
    static getDerivedStateFromProps(nextProps: any, prevState: any): any;
    constructor(props: any);
    addActive(): any;
    check(...args: any[]): void;
    componentDidMount(): void;
    componentDidUpdate(prevProps: any, prevState: any): void;
    componentWillUnmount(): void;
    forceUpdate(callback: any): void;
    getIndex(list: any, val: any): any;
    handleCheckEnd(err: any, message: any): void;
    onBlur(e: any): void;
    onChange(value: any, e: any): void;
    onClick(e: any): void;
    onFocus(e: any): void;
    onKeyDown(e: any): any;
    pageClick(e: any): void;
    removeActive(): void;
    render(): any;
    resetCurrentFocus(): void;
    scroll(...args: any[]): void;
    setState(partialState: any, callback: any): void;
    setTimeoutTyping(): void;
    toggleShow(show: any): void;
}
export namespace Select {
    namespace defaultProps {
        const classNameContainer: string;
        const classNameDropdownIconOptionListItem: string;
        const classNameOptionListContainer: string;
        const classNameOptionListItem: string;
        const classNameWrapper: string;
        const customStyleContainer: {};
        const customStyleDropdownIcon: {};
        const customStyleOptionListContainer: {};
        const customStyleOptionListItem: {};
        const customStyleWrapper: {};
        const disabled: boolean;
        const id: string;
        const name: string;
        function onChange(): void;
        const optionList: any[];
        const tabIndex: number;
        const validate: boolean;
        const validationOption: {};
        const value: string;
    }
}
export class Textarea {
    static getDerivedStateFromProps(nextProps: any, prevState: any): any;
    constructor(props: any);
    check(...args: any[]): void;
    componentDidUpdate(prevProps: any, prevState: any): void;
    forceUpdate(callback: any): void;
    handleCheckEnd(err: any, message: any): void;
    onBlur(e: any): void;
    onChange(e: any): void;
    onFocus(e: any): void;
    onKeyUp(e: any): void;
    render(): any;
    setState(partialState: any, callback: any): void;
}
export namespace Textarea {
    namespace defaultProps {
        const classNameContainer: string;
        const classNameInput: string;
        const classNameWrapper: string;
        const cols: number;
        const customStyleContainer: {};
        const customStyleInput: {};
        const customStyleWrapper: {};
        const disabled: boolean;
        const id: string;
        const maxLength: number;
        const name: string;
        function onChange(): void;
        const placeholder: string;
        const rows: number;
        const tabIndex: number;
        const type: string;
        const validate: boolean;
        const validationOption: {};
        const value: string;
    }
}
export class Textbox {
    static getDerivedStateFromProps(nextProps: any, prevState: any): any;
    constructor(props: any);
    autoFormatNumber(v: any): any;
    check(...args: any[]): void;
    componentDidUpdate(prevProps: any, prevState: any): void;
    forceUpdate(callback: any): void;
    handleCheckEnd(err: any, message: any): void;
    onBlur(e: any): void;
    onChange(e: any): void;
    onFocus(e: any): void;
    onKeyUp(e: any): void;
    render(): any;
    setState(partialState: any, callback: any): void;
}
export namespace Textbox {
    namespace defaultProps {
        const autoComplete: string;
        const classNameContainer: string;
        const classNameInput: string;
        const classNameWrapper: string;
        const customStyleContainer: {};
        const customStyleInput: {};
        const customStyleWrapper: {};
        const disabled: boolean;
        const id: string;
        const maxLength: number;
        const name: string;
        function onChange(): void;
        const placeholder: string;
        const tabIndex: number;
        const type: string;
        const validate: boolean;
        const validationOption: {};
        const value: string;
    }
}