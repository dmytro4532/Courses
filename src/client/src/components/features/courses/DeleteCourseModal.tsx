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
      enqueueSnackbar('Course deleted successfully', { variant: 'success', autoHideDuration: 3000 });
      onSuccess();
    } catch (error) {
      enqueueSnackbar('Failed to delete course', { variant: 'error', autoHideDuration: 3000 });
      console.error('Error deleting course:', error);
    }
  };

  return (
    <Modal
      title="Delete Course"
      open={!!course}
      onCancel={onClose}
      onOk={handleDelete}
      okText="Delete"
      okButtonProps={{
        danger: true,
      }}
      cancelText="Cancel"
    >
      <p>Are you sure you want to delete the course "{course?.title}"?</p>
      <p>This action cannot be undone.</p>
    </Modal>
  );
}; 