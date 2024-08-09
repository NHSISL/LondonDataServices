import { faCheck, faTimes, faFileDownload } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { FunctionComponent } from "react";
import moment from "moment";
import { IngestionTrackingView } from "../../models/views/components/ingestionTracking/ingestionTrackingView";
import ButtonBase from "../bases/buttons/ButtonBase";
import CardBase from "../bases/components/Card/CardBase";
import CardBaseBody from "../bases/components/Card/CardBase.Body";
import CardBaseTitle from "../bases/components/Card/CardBase.Title";
import CardBaseContent from "../bases/components/Card/CardBase.Content";
import SummaryListBaseRow from "../bases/components/SummaryList/SummaryListBase.Row";
import SummaryListBaseValue from "../bases/components/SummaryList/SummaryListBase.Value";
import SummaryListBaseKey from "../bases/components/SummaryList/SummaryListBase.Key";
import SummaryListBase from "../bases/components/SummaryList/SummaryListBase";
import GridBase from "../bases/layouts/Grid/GridBase";

interface IngestionTrackingDetailCardViewProps {
    ingestionTracking: IngestionTrackingView;
    onDownload: (ingestionTracking: IngestionTrackingView) => void;
    onReDecrypt: (ingestionTracking: IngestionTrackingView) => void;
    onRefresh: (ingestionTracking: IngestionTrackingView) => void;
}

const IngestionTrackingDetailCardView: FunctionComponent<IngestionTrackingDetailCardViewProps> = (props) => {
    const {
        ingestionTracking,
        onDownload,
        onReDecrypt
    } = props;

    return (
        <>
            <div className="row">
                <GridBase size="Two-Third">
                    <CardBase>
                        <CardBaseBody>
                            <CardBaseTitle>Details</CardBaseTitle>
                            <CardBaseContent>
                                <SummaryListBase>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Supplier</SummaryListBaseKey>
                                        <SummaryListBaseValue>{ingestionTracking.supplier?.name}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Encrypted File Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {ingestionTracking.encryptedFileName}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Decrypted File Name</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {ingestionTracking.decryptedFileName}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Decrypted</SummaryListBaseKey>
                                        <SummaryListBaseValue>{ingestionTracking.decrypted ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Last Seen</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(ingestionTracking.lastSeen?.toString()).format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>File Deleted</SummaryListBaseKey>
                                        <SummaryListBaseValue>{ingestionTracking.fileDeleted ?
                                            <FontAwesomeIcon icon={faCheck} className="text-success" /> :
                                            <FontAwesomeIcon icon={faTimes} className="text-danger" />}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Record Count</SummaryListBaseKey>
                                        <SummaryListBaseValue>{ingestionTracking.recordCount}</SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Encrypted File Size</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {ingestionTracking.encryptedFileSize}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Decrypted File Size</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {ingestionTracking.decryptedFileSize}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Created Date</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(ingestionTracking.createdDate?.toString()).format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                    <SummaryListBaseRow>
                                        <SummaryListBaseKey>Updated Date</SummaryListBaseKey>
                                        <SummaryListBaseValue>
                                            {moment(ingestionTracking.updatedDate?.toString()).format("Do-MMM-yyyy HH:mm")}
                                        </SummaryListBaseValue>
                                    </SummaryListBaseRow>
                                </SummaryListBase>
                            </CardBaseContent>
                        </CardBaseBody>
                    </CardBase>
                </GridBase>

                {(ingestionTracking.supplier?.canDownloadIngestionTracking || ingestionTracking.supplier?.canRelandIngestionTracking) && (
                    <>
                        <GridBase size="One-Third">
                            <CardBase>
                                <CardBaseBody>
                                    <CardBaseTitle>
                                        Actions
                                    </CardBaseTitle>
                                    <CardBaseContent>
                                        <ul className="ps-0 mb-4">
                                            <li>
                                                <span>1</span> Re-Encrypt
                                                <p>Use this option to re-encrypt a file.</p>
                                            </li>
                                            <li>
                                                <span>3</span> Download Decrypted
                                                <p>Use this option to download a successfully decrypted file.</p>
                                            </li>
                                        </ul>
                                        {ingestionTracking.decrypted && (
                                            <>
                                                <ButtonBase onClick={() => onReDecrypt(ingestionTracking)} add>
                                                    &nbsp;Re-Decrypt
                                                </ButtonBase> &nbsp;
                                            </>
                                        )}

                                        <ButtonBase onClick={() => onDownload(ingestionTracking)} add>
                                            <FontAwesomeIcon icon={faFileDownload} />&nbsp;Download
                                        </ButtonBase>&nbsp;

                                    </CardBaseContent>
                                </CardBaseBody>
                            </CardBase>
                        </GridBase>
                    </>
                )}
            </div>
        </>
    );
}
export default IngestionTrackingDetailCardView;