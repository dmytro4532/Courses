import { Modal } from 'antd';
import { enqueueSnackbar } from 'notistack';
import api from '../../../api/axios';
import type { CourseResponse } from '../../../types';

interface DeleteCourseModalProps {
  course: CourseResponse | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteCourseModal = ({ course, onClose, onSuccess }: DeleteCourseModalProps) => {
  const handleDelete = async () => {
    if (!course) return;

    try {
      await api.delete(`/api/courses/${course.id}`);
      enqueueSnackbar('Курс успішно видалено', { variant: 'success', autoHideDuration: 3000 });
      onSuccess();
    } catch (error) {
      enqueueSnackbar('Не вдалося видалити курс', { variant: 'error', autoHideDuration: 3000 });
    }
  };

  return (
    <Modal
      title="Видалити курс"
      open={!!course}
      onCancel={onClose}
      onOk={handleDelete}
      okText="Видалити"
      okButtonProps={{
        danger: true,
      }}
      cancelText="Скасувати"
    >
      <p>Ви впевнені, що хочете видалити курс "{course?.title}"?</p>
      <p>Цю дію неможливо скасувати.</p>
    </Modal>
  );
}; 