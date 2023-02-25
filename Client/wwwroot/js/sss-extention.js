export const getSssObject = async () => {

    await new Promise(resolve => setTimeout(resolve, 200));
    
    if (!window.isAllowedSSS()) {
        window.requestSSS();
    }

    return window.SSS;
}