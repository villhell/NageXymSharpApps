
export async function GetSssObject(){

    await new Promise(resolve => setTimeout(resolve, 200));
    
    if (window.isAllowedSSS()) {
        return window.SSS;
    }
}