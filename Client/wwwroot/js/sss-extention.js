export const getSssObject = () => {
    if (window.SSS) {
        return window.SSS;
    } else {
        return null;
    }
}