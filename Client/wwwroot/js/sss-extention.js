
export async function GetSssObject(){

    if (window.isAllowedSSS()) {
        return window.SSS;
    }
}