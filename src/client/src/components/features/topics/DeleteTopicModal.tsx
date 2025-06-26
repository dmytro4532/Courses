import { Modal } from 'antd';
import { enqueueSnackbar } from 'notistack';
import api from '../../../api/axios';
import type { Topic } from '../../../types';

interface DeleteTopicModalProps {
  topic: Topic | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteTopicModal = ({ topic, onClose, onSuccess }: DeleteTopicModalProps) => {
  const handleDelete = async () => {
    if (!topic) return;

    try {
      await api.delete(`/api/topics/${topic.id}`);
      enqueueSnackbar('Тему успішно видалено', { variant: 'success', autoHideDuration: 3000 });
      onSuccess();
    } catch (error: any) {
      enqueueSnackbar(error?.response?.data?.details || 'Не вдалося видалити тему', { variant: 'error', autoHideDuration: 3000 });
    }
  };

  return (
    <Modal
      title="Видалити тему"
      open={!!topic}
      onCancel={onClose}
      onOk={handleDelete}
      okText="Видалити"
      okButtonProps={{
        danger: true,
      }}
      cancelText="Скасувати"
    >
      <p>Ви впевнені, що хочете видалити тему "{topic?.title}"?</p>
      <p>Цю дію неможливо скасувати.</p>
    </Modal>
  );
}; 