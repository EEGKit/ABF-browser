import React from 'react';

/**
 * Shows a single parents (with color/comments)
 */
class ParentListItem extends React.Component {

    render() {

        if (this.props.cell == null)
            return <div>no cell</div>

        // TODO: ew this is yuck
        const parentID = this.props.cell["parentID"];
        const childCount = this.props.cell["abfPaths"].length;
        const color = this.props.cell["color"];
        const comment = this.props.cell["comment"];
        const comment2 = comment === "?" ? null : comment;

        if (childCount === 0)
            return null;

        return (
            <div key={parentID} className='my-0' style={{ fontSize: '.8em' }}>

                <span className='px-1 font-monospace' style={{ backgroundColor: color, padding: '3px' }}>
                    <span style={{ cursor: 'pointer' }} onClick={() => this.props.onClick()}>{parentID}</span>
                </span>

                <div style={{ display: 'inline-block', opacity: .5 }}>
                    &nbsp;({childCount})&nbsp;
                </div>

                <div style={{ display: 'inline-block', fontFamily: 'arial narrow', opacity: .5 }}>
                    {comment2}
                </div>

            </div>
        );
    }
}

export default ParentListItem;