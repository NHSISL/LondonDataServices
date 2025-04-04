import { useEffect, useState } from 'react';
import { Pds } from '../../../models/pds/pds';
import { PdsView } from '../../../models/views/components/pds/pdsView';
import { pdsService } from '../../foundations/pdsService';

export const pdsViewService = {

    useGetAllPds: (searchTerm?: string) => {
    
            let query = `?$expand=audit&$orderby=createdDate`;

            if (searchTerm) {
                query = query + `&$filter=contains(Id,'${searchTerm}')`;
            }

            const response = pdsService.useGetAllPds(query);
            const [mappedPds, setMappedPds] = useState<Array<PdsView>>();

            useEffect(() => {
                if (response.data) {
                    const pds = response.data.map((pds: Pds) => new PdsView(
                        pds.id,
                        pds.correlationId,
                        pds.message,
                        pds.fileName,
                        pds.messageId,
                        pds.createdDate,
                        pds.createdBy,
                        pds.updatedDate,
                        pds.updatedBy,
                    ));

                    setMappedPds(pds);
                }
            }, [response.data]);

            return {
                mappedPds, ...response
            }
    }
}