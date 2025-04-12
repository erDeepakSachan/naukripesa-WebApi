export interface Menu {
    ControllerName: string | null,
    Icon: string | null,
    IconColor: string | null,
    IconCss: string | null,
    NavUrl: string | null,
    Text: string | null,
    Nodes: Menu[] | null,
}

export const emptyMenu = (): Menu => {
    return {
        ControllerName: '',
        Icon: '',
        IconColor: '',
        IconCss: '',
        NavUrl: '',
        Text: '',
        Nodes: [],
    };
};