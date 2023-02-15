import { getActiveNode } from "symbol-node-util";
import { SSSWindow, setTransactionByPayload, requestSign, getActiveAddress } from "sss-module";

export async function GetActiveNode(network) {
    return await getActiveNode(network);
}

export async function SetTransactionByPayload(payload) {
    setTransactionByPayload(payload);
    return await requestSign();
}

export async function RequestSign() {
    return await requestSign();
}

export function GetActiveAddress() {
    return getActiveAddress();
}