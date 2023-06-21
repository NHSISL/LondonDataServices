import { faUpload } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React, { ChangeEvent, FunctionComponent, useState } from "react";
import ButtonBase from "../../bases/buttons/ButtonBase";

interface OptOutUploadDetailCardViewProps {
    onUpload: (data: string[]) => void;
}

const OptOutUploadDetailCardView: FunctionComponent<OptOutUploadDetailCardViewProps> = (props) => {
    const {
        onUpload 
    } = props;

    const [file, setFile] = useState<File | null>(null);
    const [nhsNumbers, setNhsNumbers] = useState<string[]>([]);

    const splitCSVRows = (csvContent: string) => {
        const rows = csvContent.split('\n');
        const nhsNumbers = rows.map((row) => row.replace(/"/g, '').split(',')[0]).filter((row) => row.trim() !== '');
        return nhsNumbers;
    };

    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        const selectedFile = event.target.files?.[0];
        if (selectedFile && selectedFile.type === 'text/csv') {
            setFile(selectedFile);
            const reader = new FileReader();
            reader.onload = () => {
                const result = reader.result?.toString();
                const nhsNumbers = splitCSVRows(result || '');
                setNhsNumbers(nhsNumbers);
            };
            reader.readAsText(selectedFile);
        }
    }
    const handleUploadClick = () => {
        if (!file) {
            return;
        }
        onUpload(nhsNumbers);

    }

    return (
        <>
            <div>
                <input type="file" onChange={handleFileChange} /> <br />

                {nhsNumbers.length > 0 && (
                    <div>
                        <br />
                        <ButtonBase onClick={handleUploadClick} add>
                            <FontAwesomeIcon icon={faUpload} />
                            &nbsp;Upload below NHS Numbers
                        </ButtonBase>
                        <br />
                        <h3>Uploaded NHS numbers:</h3>
                        <ul>
                            {nhsNumbers.map((nhsNumber, index) => (
                                <li key={index}>{nhsNumber}</li>
                            ))}
                        </ul>
                       
                    </div>
                )}

            </div>
        </>
    );
}

export default OptOutUploadDetailCardView;