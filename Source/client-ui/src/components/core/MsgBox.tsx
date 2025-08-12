import React, { useState, useEffect } from 'react';
import { Dialog } from 'primereact/dialog';
import { Button } from 'primereact/button';

export type MessageBoxType = 'info' | 'success' | 'warn' | 'error' | 'confirm';

export interface MessageBoxProps {
  type: MessageBoxType;
  title: string;
  message: string;
  visible: boolean;
  onHide: () => void;
  onConfirm?: () => void;
  confirmLabel?: string;
  cancelLabel?: string;
}

const MessageBox: React.FC<MessageBoxProps> = ({
  type,
  title,
  message,
  visible,
  onHide,
  onConfirm,
  confirmLabel = 'Да',
  cancelLabel = 'Отмена'
}) => {
  const [icon, setIcon] = useState<string>('');
  const [color, setColor] = useState<string>('');

  useEffect(() => {
    switch (type) {
      case 'info':
        setIcon('pi pi-info-circle');
        setColor('text-primary-500');
        break;
      case 'success':
        setIcon('pi pi-check-circle');
        setColor('text-green-500');
        break;
      case 'warn':
        setIcon('pi pi-exclamation-triangle');
        setColor('text-yellow-500');
        break;
      case 'error':
        setIcon('pi pi-times-circle');
        setColor('text-red-500');
        break;
      case 'confirm':
        setIcon('pi pi-question-circle');
        setColor('text-blue-500');
        break;
      default:
        setIcon('pi pi-info-circle');
        setColor('text-primary-500');
    }
  }, [type]);

  const renderFooter = () => {
    if (type === 'confirm') {
      return (
        <div>
          <Button 
            label={cancelLabel} 
            icon="pi pi-times" 
            onClick={onHide} 
            className="p-button-text" 
          />
          <Button 
            label={confirmLabel} 
            icon="pi pi-check" 
            onClick={() => {
              onConfirm?.();
              onHide();
            }} 
            autoFocus 
          />
        </div>
      );
    }
    return (
      <Button 
        label="OK" 
        icon="pi pi-check" 
        onClick={onHide} 
        autoFocus 
      />
    );
  };

  return (
    <Dialog
      header={title}
      visible={visible}
      style={{ width: '450px' }}
      footer={renderFooter()}
      onHide={onHide}
      dismissableMask={type !== 'confirm'}
    >
      <div className="flex align-items-center">
        <i className={`${icon} text-4xl mr-3 ${color}`} />
        <span className="text-lg">{message}</span>
      </div>
    </Dialog>
  );
};

export default MessageBox;