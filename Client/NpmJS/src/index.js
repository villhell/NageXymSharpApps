import { getActiveNode } from "symbol-node-util";

export async function GetActiveNode(network) {
    return await getActiveNode(network);
}

export async function SetTransactionByPayload(payload, dotNetHelper) {

    window.SSS.setTransactionByPayload(payload);

    const signedTx = await window.SSS.requestSign();
    await dotNetHelper.invokeMethodAsync("Client", "GetSignedTransaction", signedTx.payload);
}