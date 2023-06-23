type AvatarProps = {
    initials: string;
};

const Avatar = ({
    initials,
}: AvatarProps) => {

    return (
        <div
            className="flex items-center justify-center w-8 h-8 rounded-full text-white font-semibold text-xs uppercase bg-blue-600"
        >
            {initials}
        </div>
    );
};

export default Avatar;