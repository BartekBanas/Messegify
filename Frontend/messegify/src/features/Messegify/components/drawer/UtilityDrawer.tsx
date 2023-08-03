import React, {FC} from 'react';
import {useDisclosure} from "@mantine/hooks";
import {Drawer, Group, ActionIcon, Flex} from "@mantine/core";
import {ContactMaker} from "../contactMaker/ContactMaker";
import {IconAdjustments} from '@tabler/icons-react';
import {DeleteAccountButton} from "./deleteAccountButton";

interface UtilityDrawerProps {
}

export const UtilityDrawer: FC<UtilityDrawerProps> = () => {
    const [opened, {open, close}] = useDisclosure(false);

    return (
        <>
            <Drawer opened={opened} onClose={close}>
                <Flex
                    mih={300}
                    gap="xl"
                    justify="center"
                    align="center"
                    direction="column"
                    wrap="wrap"
                >
                    <ContactMaker/>
                    <DeleteAccountButton/>
                </Flex>
            </Drawer>

            <Group position="center">
                <ActionIcon onClick={() => open()} size="xl">
                    <IconAdjustments size="2rem"/>
                </ActionIcon>
            </Group>
        </>
    );
};