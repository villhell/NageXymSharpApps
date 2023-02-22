import { getActiveNode } from "symbol-node-util";

export async function GetActiveNode(network) {
    return await getActiveNode(network);
}

export function GetSssObject() {
    if (window.SSS !== undefined) {
        return window.SSS;
    } else {
        return null;
    }
}

export async function SetTransactionByPayload(payload) {

    window.SSS.setTransactionByPayload(payload);

    const signedTx = await window.SSS.requestSign();
    DotNet.invokeMethod("BlazorNpmSample", "GetSignedTransaction", signedTx.payload);
}